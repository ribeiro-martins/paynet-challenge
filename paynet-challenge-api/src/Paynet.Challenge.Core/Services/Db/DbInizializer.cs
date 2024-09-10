using Paynet.Challenge.Core.Services.User;
using Paynet.Challenge.DataContract.V1.User;
using Paynet.Challenge.Entities.CommonTypes;

namespace Paynet.Challenge.Core.Services.Db
{
    public class DbInizializer
    {
        private readonly IUserService _userService;

        private readonly CreateUserDto _defaultUser = new CreateUserDto
        {
            Email = "admin@paynet.com.br",
            FullName = "Administrador Paynet",
            Password = "vowedida",
            Address = new Address
            {
                City = "Belford Roxo",
                Neighborhood = "Piam",
                Number = "179999",
                State = "RJ",
                ZipCode = "26120180",
                Street = "Rua Owasdo"
            },
            ConfirmPassword = "vowedida"  
        };

        public DbInizializer(IUserService userService)
        {
            _userService = userService;
        }

        public void Init()
        {
            var user = _userService.GetByEmail(_defaultUser.Email);
            if (user == null)
            {
                _userService.Add(_defaultUser);
            }
        }
    }
}