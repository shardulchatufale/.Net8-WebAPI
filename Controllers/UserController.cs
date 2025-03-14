using DotNet8WebAPI.Helper;
using DotNet8WebAPI.Model;
using DotNet8WebAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetALL());
        }

        [HttpPost]
        
        public async Task<IActionResult> Post([FromBody] User userObj)
        {
            userObj.Id = 0;
            return Ok(await _userService.AddAndUpdateUser(userObj));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] User userObj)
        {
            userObj.Id = id;
            return Ok(await _userService.AddAndUpdateUser(userObj));
        }

        [HttpPost("Login")]
        public async Task <IActionResult>Login(AuthenticationRequest Model)
        {
            var response = await _userService.Authenticate(Model);
            if (response == null) { return BadRequest(new { Message = "Username or Password is incorrect" }); }
            return Ok(response);
        }
    }
}
