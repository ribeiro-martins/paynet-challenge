using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation.Results;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Paynet.Challenge.Core.ErrorHandling.ErrorMessages;
using Paynet.Challenge.Core.Security;
using Paynet.Challenge.Core.Services.Email;
using Paynet.Challenge.Core.Settings;
using Paynet.Challenge.Core.Templates.ForgotPassword;
using Paynet.Challenge.Core.Utils;
using Paynet.Challenge.DataContract.V1;
using Paynet.Challenge.DataContract.V1.Auth;
using Paynet.Challenge.Entities.CommonTypes;
using Paynet.Challenge.Entities.User;
using Paynet.Challenge.Repository;

namespace Paynet.Challenge.Core.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        private readonly IEmailService _emailService;

        private readonly ISettings _settings;


        public AuthService(
            IUserRepository userRepository, 
            ISettings settings,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _settings = settings;
        }

        public LoginResponse Login(LoginContract login)
        {
            LoginResponse operationResponse = new();
            if (!login.Validate(out ValidationResult validationResult))
            {
                foreach (var error in validationResult.Errors)
                {
                    operationResponse.AddGenericFieldsValidationError(error.PropertyName, error.ErrorMessage);
                }

                return operationResponse;
            }
            var user = _userRepository.GetByEmail(login.Email);
            if (user == null || !SecurePasswordHasher.Verify(login.Password, user.Password))
            {
                operationResponse.AddInvalidUserOrPasswordError();
                return operationResponse;

            }

            operationResponse.AccessToken = GenerateToken(user);
            return operationResponse;
        }

        private string GenerateToken(UserEntity user)
        {
            var handler = new JwtSecurityTokenHandler();
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SigninKey));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                ]),
                Expires = DateTime.UtcNow.AddMinutes(_settings.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = _settings.Issuer,
                Audience = _settings.Audience
            };

            var securityToken = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(securityToken);
        }

        public OperationResponse ForgotPassword(ForgotPasswordContract forgotPasswordContract)
        {
            var operationResponse = new OperationResponse();

            if (!forgotPasswordContract.Validate(out ValidationResult validationResult))
            {
                foreach (var error in validationResult.Errors)
                {
                    operationResponse.AddGenericFieldsValidationError(error.PropertyName, error.ErrorMessage);
                }

                return operationResponse;
            }

            var user = _userRepository.GetByEmail(forgotPasswordContract.Email);
            if (user != null)
            {
                if (user.ForgetPasswordsInformation == null)
                {
                    user.ForgetPasswordsInformation = new List<ForgetPasswordInformation>();
                }

                if (user.ForgetPasswordsInformation.Count >= 3)
                {
                    var oldestCode = user.ForgetPasswordsInformation.OrderBy(info => info.RequestDateTime).First();
                    user.ForgetPasswordsInformation.Remove(oldestCode);
                }

                string hashedPassword = SecurePasswordHasher.Hash(forgotPasswordContract.NewPassword);
                string randomCode = RandomCodeGenerator.GenerateRandomCode();

            _emailService.SendEmail(
                forgotPasswordContract.Email, 
                "Código para Recuperação da Conta", 
                ForgotPasswordTemplates.FORGOT_PASSWORD_TEMPLATE.Replace("{{randomCode}}", randomCode),
                true);


                var newForgetPasswordInfo = new ForgetPasswordInformation
                {
                    SecretCode = randomCode,
                    RequestDateTime = DateTime.UtcNow,
                    Password = hashedPassword
                };
            
                user.ForgetPasswordsInformation.Add(newForgetPasswordInfo);

                var update = Builders<UserEntity>.Update
                    .Set(u => u.ForgetPasswordsInformation, user.ForgetPasswordsInformation);

                _userRepository.UpdateOneByEmail(forgotPasswordContract.Email, update);
            }

            return new OperationResponse();
        }

        public OperationResponse VerifyForgotCode(VerifyForgotPasswordContract verifyForgotPasswordContract)
        {
            var operationResponse = new OperationResponse();
            var user = _userRepository.GetByEmail(verifyForgotPasswordContract.Email);

            if (user == null || user.ForgetPasswordsInformation == null)
            {
                operationResponse.AddInvalidForgotPasswordCodeError();
                return operationResponse;
            }

            var forgotPasswordCodeFound = user.ForgetPasswordsInformation.FirstOrDefault(f => f.SecretCode == verifyForgotPasswordContract.SecretCode);

            if (forgotPasswordCodeFound != null)
            {
                var timeElapsed = DateTime.UtcNow - forgotPasswordCodeFound.RequestDateTime;
                if (timeElapsed.TotalHours <= 1)
                {
                    var update = Builders<UserEntity>.Update
                        .Set(u => u.ForgetPasswordsInformation, new List<ForgetPasswordInformation>())
                        .Set(u => u.Password, forgotPasswordCodeFound.Password);

                    _userRepository.UpdateOneByEmail(verifyForgotPasswordContract.Email, update);
                    return operationResponse;
                }
            }

            operationResponse.AddInvalidForgotPasswordCodeError();
            return operationResponse;
        }

    }
}