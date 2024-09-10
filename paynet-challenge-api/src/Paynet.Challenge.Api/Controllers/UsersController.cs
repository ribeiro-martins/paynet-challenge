using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paynet.Challenge.Core.Services.User;
using Paynet.Challenge.DataContract.V1.User;

namespace Paynet.Challenge.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add([FromBody] CreateUserDto user)
        {
            var response = _userService.Add(user);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Created();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _userService.GetAll();
            return Ok(response);
        }
    }
}
