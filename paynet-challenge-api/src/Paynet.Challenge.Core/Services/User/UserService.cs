using Paynet.Challenge.Core.ErrorHandling.ErrorMessages;
using Paynet.Challenge.Core.Mappers;
using Paynet.Challenge.Core.Security;
using Paynet.Challenge.DataContract.V1;
using Paynet.Challenge.DataContract.V1.User;
using Paynet.Challenge.Repository;
using FluentValidation.Results;
using Paynet.Challenge.Core.Services.Cep;
using MongoDB.Driver;

namespace Paynet.Challenge.Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly ICepService _cepService;

        public UserService(IUserRepository userRepository, ICepService cepService)
        {
            _userRepository = userRepository;
            _cepService = cepService;
        }

        public OperationResponse Add(CreateUserDto user)
        {
            var operationResponse = new OperationResponse();

            if (!user.Validate(out ValidationResult validationResult))
            {
                foreach (var error in validationResult.Errors)
                {
                    operationResponse.AddGenericFieldsValidationError(error.PropertyName, error.ErrorMessage);
                }

                return operationResponse;
            }

            if (user.Password != user.ConfirmPassword)
            {
                operationResponse.AddUserConfirmPasswordMustBeEqualError();
                return operationResponse;
            }

            var foundUser = _userRepository.GetByEmail(user.Email);
            if (foundUser != null)
            {
                operationResponse.AddUserAlreadyExistsError();
                return operationResponse;
            }

            bool isValid = _cepService.IsCepValid(user.Address.ZipCode);
            if (!isValid)
            {
                operationResponse.AddCepNotFoundError();
                return operationResponse;
            }


            string hashedPassword = SecurePasswordHasher.Hash(user.Password);
            _userRepository.Add(user.CreateUserDtoToUser(hashedPassword));
            return operationResponse;
        }

        public GetAllUsersResponse GetAll()
        {
            var users = _userRepository.GetAll().ToList();
            return new GetAllUsersResponse
            {
                Users = users.UsersToUsersDto()
            };
        }

        public UserWithPasswordDto GetByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
            return user.UserToUserWithPasswordDto();
        }
    }
}