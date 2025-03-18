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
        private readonly IUserService _userService;

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

        [HttpGet("{id}")]
        [Authorize] 
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByID(id);

            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User userObj)
        {
            userObj.Id = 0;
            var result = await _userService.AddAndUpdateUser(userObj);
            if (result == null)
            {
                return BadRequest(new { Message = "Failed to create the user." });
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] User userObj)
        {
            userObj.Id = id;
            var result = await _userService.AddAndUpdateUser(userObj);
            if (result == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthenticationRequest model)
        {
            var response = await _userService.Authenticate(model);
            if (response == null)
            {
                return BadRequest(new { Message = "Username or Password is incorrect." });
            }
            return Ok(response);
        }
    }
}
