using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    public class PartieController : Controller
    {
        //---------------------INJECTION BLL-------------------------------------------------------------------------------------


        private readonly PartieBLL _partieBLL;

        public PartieController(PartieBLL partieBLL)
        {
            _partieBLL = partieBLL;
        }

        //---------------------------------GET ALL INFO OF A PARTIE BY ID---------------------------------------------------------------


        [HttpGet("{id}")]
        public async Task<ActionResult<Partie>> GetPartieById(int id)
        {
            try
            {
                var partie = await _partieBLL.GetPartieById(id);
                if (partie == null) return NotFound(new { message = "Aucune partie trouvée avec cet ID." });

                return Ok(partie);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        //---------------------------------GET ALL  PARTIEs---------------------------------------------------------------

        [HttpGet("AllParties")]  //Méthode get avec un paramètre url id
        public async Task<ActionResult<List<Partie>>> GetAllParties() 
        {
            try
            {
                var partie = await _partieBLL.GetAllParties();
                return Ok(partie);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });//ce format renvoie en json alors que juste badRequest(ex.Message) juste string
            }
        }

        //--------------------------ADD PARTIE-----------------------------------------------------------------------------


        [HttpPost("addPartie")]

        public async Task<ActionResult>AddGame(Boolean amical)
        {
            try
            {
                await _partieBLL.AddPartie(amical);
                return Ok("Partie ajouter");
            }
            catch(Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        //--------------------------ADD JOUE-----------------------------------------------------------------------------


        [HttpPost("addJoue")]
        public async Task<ActionResult> AddJoue(int joueurId, int partieId, int points)
        {
            try
            {
                await _partieBLL.AddJoue(joueurId, partieId, points);
                return Ok("joueur ajouter a une partie ainsi que ces points");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
