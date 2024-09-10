namespace Paynet.Challenge.Entities.CommonTypes
{
    public class ForgetPasswordInformation
    {
        public string Password { get; set; }

        public DateTime RequestDateTime { get; set; }

        public string SecretCode { get; set; }
    }
}