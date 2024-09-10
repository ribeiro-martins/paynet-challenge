using Paynet.Challenge.DataContract.V1;
using Paynet.Challenge.DataContract.V1.Auth;

namespace Paynet.Challenge.Core.Services.Auth
{
    public interface IAuthService
    {
        public LoginResponse Login(LoginContract login);

        OperationResponse ForgotPassword(ForgotPasswordContract forgotPasswordContract);

        OperationResponse VerifyForgotCode(VerifyForgotPasswordContract verifyForgotPasswordContract);
    }
}