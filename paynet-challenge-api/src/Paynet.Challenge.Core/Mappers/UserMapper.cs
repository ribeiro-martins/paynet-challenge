using Paynet.Challenge.DataContract.V1.User;
using Paynet.Challenge.Entities.User;

namespace Paynet.Challenge.Core.Mappers
{
    public static class UserMapper
    {
        public static UserEntity CreateUserDtoToUser(this CreateUserDto user, string hashedPassword)
        {
            return new UserEntity
            {
                Address = user.Address,
                Email = user.Email,
                Password = hashedPassword,
                FullName = user.FullName
            };
        }

        public static UserDto UserToUserDto(this UserEntity user)
        {
            if (user == null)
                return null;

            return new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id.ToString()
            };
        }

        public static List<UserDto> UsersToUsersDto(this List<UserEntity> users)
        {
            return users.Select(t => t.UserToUserDto()).ToList();
        }

        public static UserWithPasswordDto UserToUserWithPasswordDto(this UserEntity user)
        {
            if (user == null)
                return null;

            return new UserWithPasswordDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Password = user.Password,
                Id = user.Id.ToString()
            };
        }
    }
}