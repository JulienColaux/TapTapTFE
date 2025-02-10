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
                return BadRequest(new { message = ex.Message });//ce format renvoie en json alors que juste badRequest(ex.Message) juste string
            }
        }


        //-------------------------ADD POINTS--------------------------------------------------------------------------------

        public class PointsDto
        {
            public int JoueurId { get; set; }

            public int SeasonId { get; set; }
            public int Points { get; set; }
        }


        [HttpPost("addPoints")]
        public async Task<IActionResult> addPoints([FromBody] PointsDto dto)
        {
            try
            {
                await _joueurBLL.AddPoints(dto.JoueurId, dto.SeasonId, dto.Points);
                return Ok("Points mis à jour");
            }
             catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        //-------------------------ADD XP--------------------------------------------------------------------------------

        public class XPDto
        {
            public int JoueurId { get; set; }
            public int Xp { get; set; }
        }


        [HttpPost("addXp")]
        public async Task<IActionResult> addXp([FromBody] XPDto dto)
        {
            try
            {
                await _joueurBLL.AddXP(dto.JoueurId, dto.Xp);
                return Ok("XP mis à jour");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //--------------------------GET TROPHEES------------------------------------------------------------------------------------

        //A voir si je supprime pas vu que cest dans joueur mais pour le moment je laisse


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

        //-----------------------------------RESET SEASON POINT OF JOUEUR---------------------------------------------------------------



        [HttpPost("ResetElo")]
        public async Task<IActionResult> ResetPointsById(int joueurId)
        {
            try
            {
                await _joueurBLL.ResetPointsById(joueurId);
                return Ok("Reset points OK");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}

