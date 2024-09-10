using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using Moq;
using Paynet.Challenge.Core.ErrorHandling;
using Paynet.Challenge.Core.Security;
using Paynet.Challenge.Core.Services.Auth;
using Paynet.Challenge.Core.Services.Email;
using Paynet.Challenge.Core.Services.User;
using Paynet.Challenge.Core.Settings;
using Paynet.Challenge.DataContract.V1.Auth;
using Paynet.Challenge.DataContract.V1.User;
using Paynet.Challenge.Entities.User;
using Paynet.Challenge.Repository;

namespace Paynet.Challenge.UnitTests.Services.Auth
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;

        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ISettings> _mockSettings;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _mockSettings = new Mock<ISettings>();

            _mockSettings.Setup(s => s.SigninKey).Returns("a6765173-f01d-4fe4-bb8d-44ca5952f8c6");
            _mockSettings.Setup(s => s.Issuer).Returns("issuer");
            _mockSettings.Setup(s => s.Audience).Returns("audience");
            _mockSettings.Setup(s => s.ExpirationInMinutes).Returns(60);

            _authService = new AuthService(_mockUserRepository.Object, _mockSettings.Object, _mockEmailService.Object);
        }

        [Fact]
        public void Login_ShouldReturnAccessToken_WhenLoginIsValid()
        {
            // Arrange
            var login = new LoginContract
            {
                Email = "test@example.com",
                Password = "CorrectPassword"
            };

            var user = new UserEntity
            {
                Email = login.Email,
                FullName = "Antônio Testeiro",
                Password = SecurePasswordHasher.Hash("CorrectPassword")
            };

            _mockUserRepository.Setup(us => us.GetByEmail(login.Email)).Returns(user);
            _mockEmailService.Setup(us => us.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            // Act
            var result = _authService.Login(login);

            // Assert
            result.AccessToken.Should().NotBeNullOrEmpty();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.AccessToken);
            token.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == login.Email);
            result.Success.Should().BeTrue();
        }

        [Fact]
        public void Login_ShouldReturnValidationErrors_WhenLoginIsInvalid()
        {
            // Arrange
            var login = new LoginContract
            {
                Email = string.Empty,
                Password = string.Empty
            };

            // Act
            var result = _authService.Login(login);

            // Assert
            result.Errors.Should().NotBeEmpty();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Field == "Email");
            result.Errors.Should().Contain(e => e.ErrorCode == ApplicationErrors.Errors["GenericFieldsValidationError"]);
            result.Errors.Should().Contain(e => e.Field == "Password");
        }

        [Fact]
        public void Login_ShouldReturnInvalidUserOrPasswordError_WhenUserDoesNotExists()
        {
            // Arrange
            var login = new LoginContract
            {
                Email = "test@example.com",
                Password = "22312dasdasdawe22"
            };


            _mockUserRepository.Setup(us => us.GetByEmail(login.Email)).Returns((UserEntity)null);

            // Act
            var result = _authService.Login(login);

            // Assert
            result.Errors.Should().NotBeEmpty();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorCode == ApplicationErrors.Errors["InvalidUserOrPassword"]);
        }

        [Fact]
        public void Login_ShouldReturnInvalidUserOrPasswordError_WhenPasswordIsIncorrect()
        {
            // Arrange
            var login = new LoginContract
            {
                Email = "test@example.com",
                Password = "invalidPassword"
            };

            var user = new UserEntity
            {
                Email = login.Email,
                FullName = "Antônio Testeiro",
                Password = SecurePasswordHasher.Hash("CorrectPassword")
            };

            _mockUserRepository.Setup(us => us.GetByEmail(login.Email)).Returns(user);

            // Act
            var result = _authService.Login(login);

            // Assert
            result.Errors.Should().NotBeEmpty();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorCode == ApplicationErrors.Errors["InvalidUserOrPassword"]);
        }
    }
}