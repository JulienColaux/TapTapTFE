using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase  //quoi la dif entre controller et controller base? ( avant c etait juste controller)
    {

        //---------------------INJECTION BLL-------------------------------------------------------------------------------------


        private readonly UserBLL _userBLL;

        public UserController(UserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        //---------------------ADD USER-----------------------------------------------------------------------------------------


        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto user)
        {
            if (_userBLL.AddUser(user.Email, user.Password))
                return Ok(new { message = "User registered successfully" });

            return BadRequest(new { message = "Registration failed" });
        }



        //-----------------------LOGIN--------------------------------------------------------------------------------------------


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDto user)
        {
            if (_userBLL.ValidateUser(user.Email, user.Password))
                return Ok(new { message = "Login successful" });

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }

}

