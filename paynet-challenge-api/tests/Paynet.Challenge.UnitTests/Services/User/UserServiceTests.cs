using Moq;
using FluentAssertions;
using Paynet.Challenge.Core.Services.User;
using Paynet.Challenge.Repository;
using Paynet.Challenge.Core.Services.Cep;
using Paynet.Challenge.DataContract.V1.User;
using Paynet.Challenge.Entities.CommonTypes;
using Paynet.Challenge.Entities.User;
using Paynet.Challenge.Core.ErrorHandling;

namespace Paynet.Challenge.UnitTests.Services.User
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ICepService> _cepServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _cepServiceMock = new Mock<ICepService>();
            _userService = new UserService(_userRepositoryMock.Object, _cepServiceMock.Object);
        }

        [Fact]
        public void Add_UserIsValid_ReturnsSuccessResponse()
        {
            // Arrange
            var userDto = new CreateUserDto
            {
                Email = "test@example.com",
                FullName = "Antôni Testeiro",
                Password = "password123",
                ConfirmPassword = "password123",
                Address = new Address
                {
                    City = "Belford Roxo",
                    ZipCode = "26120180",
                    Neighborhood = "Piam",
                    Number = "178",
                    State = "RJ",
                    Street = "Rua Alana",
                }
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Returns((UserEntity)null);
            _cepServiceMock.Setup(cep => cep.IsCepValid(It.IsAny<string>())).Returns(true);

            // Act
            var result = _userService.Add(userDto);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Fact]
        public void Add_UserAlreadyExists_ReturnsErrorResponse()
        {
            // Arrange
            var userDto = new CreateUserDto
            {
                Email = "test@example.com",
                FullName = "Antôni Testeiro",
                Password = "password123",
                ConfirmPassword = "password123",
                Address = new Address
                {
                    City = "Belford Roxo",
                    ZipCode = "26120180",
                    Neighborhood = "Piam",
                    Number = "178",
                    State = "RJ",
                    Street = "Rua Alana",
                }
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Returns(new UserEntity());

            // Act
            var result = _userService.Add(userDto);

            // Assert
            result.Errors.Should().NotBeEmpty();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorCode == ApplicationErrors.Errors["UserAlreadyExists"]);
        }

        [Fact]
        public void Add_InvalidCep_ReturnsErrorResponse()
        {
            // Arrange
            var userDto = new CreateUserDto
            {
                Email = "test@example.com",
                FullName = "Antôni Testeiro",
                Password = "password123",
                ConfirmPassword = "password123",
                Address = new Address
                {
                    City = "Belford Roxo",
                    ZipCode = "26120180",
                    Neighborhood = "Piam",
                    Number = "178",
                    State = "RJ",
                    Street = "Rua Alana",
                }
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Returns((UserEntity)null);
            _cepServiceMock.Setup(cep => cep.IsCepValid(It.IsAny<string>())).Returns(false);

            // Act
            var result = _userService.Add(userDto);

            // Assert
            result.Errors.Should().NotBeEmpty();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorCode == ApplicationErrors.Errors["CepNotFound"]);
        }

        [Fact]
        public void Add_DifferentPassword_ReturnsErrorResponse()
        {
            // Arrange
            var userDto = new CreateUserDto
            {
                Email = "test@example.com",
                FullName = "Antôni Testeiro",
                Password = "password123",
                ConfirmPassword = "23232",
                Address = new Address
                {
                    City = "Belford Roxo",
                    ZipCode = "26120180",
                    Neighborhood = "Piam",
                    Number = "178",
                    State = "RJ",
                    Street = "Rua Alana",
                }
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Returns((UserEntity)null);
            _cepServiceMock.Setup(cep => cep.IsCepValid(It.IsAny<string>())).Returns(false);

            // Act
            var result = _userService.Add(userDto);

            // Assert
            result.Errors.Should().NotBeEmpty();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorCode == ApplicationErrors.Errors["UserConfirmPasswordMustBeEqual"]);
        }
    }
}
