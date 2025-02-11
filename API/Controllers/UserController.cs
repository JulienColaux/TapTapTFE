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

        //------------------------------GET USER JOUEUR ID----------------------------------------------------------------------------


        [HttpGet("getJoueurId/{userId}")]
        public IActionResult GetJoueurId(int userId)
        {
            try
            {
                int? joueurId = _userBLL.GetJoueurIDWithUserId(userId);

                if (joueurId == null)
                {
                    return NotFound(new { message = "Joueur non trouvé pour cet utilisateur." });
                }

                return Ok(new { joueurId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Une erreur interne s'est produite." });
            }
        }


        //------------------------------GET USER  ID BY MAIL----------------------------------------------------------------------------


        [HttpGet("getUserId")]
        public IActionResult GetUserId([FromQuery] string email)
        {
            try
            {
                int? userId = _userBLL.GetUserIdByEmail(email);

                if (userId == null)
                {
                    return NotFound(new { message = "Utilisateur non trouvé pour cet email." });
                }

                return Ok(new { userId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Une erreur interne s'est produite." });
            }
        }
    }

}

