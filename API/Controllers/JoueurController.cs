using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")] // La route
    [ApiController]
    public class JoueurController : Controller
    {

        //---------------------INJECTION BLL-------------------------------------------------------------------------------------


        private readonly JoueurBLL _joueurBLL;

        public JoueurController(JoueurBLL joueurBLL)
        {
            _joueurBLL = joueurBLL;
        }


        //----------------------GET JOUEUR ALL INFO---------------------------------------------------------------------------



        [HttpGet("{id}")]  //Méthode get avec un paramètre url id
        public async Task<ActionResult<Joueur>> GetJoueur(int id)  //Action result permet de renvoyer des objet ou des code http comme 404 not found par exemple
        {
            try
            {
                var joueur = await _joueurBLL.GetJoueurDetails(id);
                return Ok(joueur);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        //-------------------------GET CLASSEMENT--------------------------------------------------------------------------------



        [HttpGet("classement")]
        public async Task<ActionResult<List<Joueur>>> GetClassement()
        {
            return Ok(await _joueurBLL.GetClassement());
        }


        //---------------------------------------------------------------------------------------------------------------------------------


        [HttpGet("trophees/{id}")]
        public async Task<IActionResult> GetAllTropheesByJoueurId(int id)
        {
            try
            {
                var trophees = await _joueurBLL.GetAllTropheesByJoueurId(id);
                if (trophees == null || trophees.Count == 0)
                {
                    return NotFound(new { message = "Aucun trophée trouvé pour ce joueur." });
                }
                return Ok(trophees);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur interne s'est produite.", details = ex.Message });
            }
        }

    }
}

