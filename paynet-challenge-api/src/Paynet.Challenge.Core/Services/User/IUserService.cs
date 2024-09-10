using Paynet.Challenge.DataContract.V1;
using Paynet.Challenge.DataContract.V1.User;

namespace Paynet.Challenge.Core.Services.User
{
    public interface IUserService
    {
        OperationResponse Add(CreateUserDto user);

        GetAllUsersResponse GetAll();

        UserWithPasswordDto GetByEmail(string email);
    }
}