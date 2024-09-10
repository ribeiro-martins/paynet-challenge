using Microsoft.AspNetCore.Mvc;
using Paynet.Challenge.Core.Services.Auth;
using Paynet.Challenge.DataContract.V1.Auth;

namespace Paynet.Challenge.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginContract login)
        {
            var response = _authService.Login(login);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordContract forgotPassword)
        {
            var response = _authService.ForgotPassword(forgotPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

         [HttpPost("verify-forgot-password")]
        public IActionResult VerifyForgotPassword([FromBody] VerifyForgotPasswordContract verifyForgotPassword)
        {
            var response = _authService.VerifyForgotCode(verifyForgotPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
