using Paynet.Challenge.Entities.CommonTypes;

namespace Paynet.Challenge.Entities.User
{
    public class UserEntity : BaseEntity
    {
        public string FullName { get; set; }

        public Address Address { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<ForgetPasswordInformation> ForgetPasswordsInformation { get; set; }
    }
}