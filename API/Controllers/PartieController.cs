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
    }
}
