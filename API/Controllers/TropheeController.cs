using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")] // La route
    [ApiController]
    public class TropheeController : Controller
    {
        //---------------------INJECTION BLL-------------------------------------------------------------------------------------


        private readonly TropheeBLL _tropheeBLL;

        public TropheeController(TropheeBLL tropheeBLL)
        {
            _tropheeBLL = tropheeBLL;
        }

        //---------------------GET TROPHEE BY ID----------------------------------------------------------------------------


        [HttpGet("{id}")]

        public async Task<ActionResult<Trophee>> GetTropheeById(int id)
        {
            try
            {
                var trophee = await _tropheeBLL.GetTropheeById(id);
                return Ok(trophee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });//ce format renvoie en json alors que juste badRequest(ex.Message) juste string
            }
        }




        [HttpGet("GetUrlImage/{id}")]
        public async Task<IActionResult> GetUrlImageTropheeByTropheeId(int id)
        {
            try
            {
                string url = await _tropheeBLL.GetUrlImageTropheeByTropheeId(id);
                if (string.IsNullOrEmpty(url))
                {
                    return NotFound($"Aucune image trouvée pour l'ID {id}");
                }
                return Ok(url);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne : {ex.Message}");
            }
        }

        //--------------------------ADD TROPHEE TO JOUEUR---------------------------------------------------------------------------------------------------


        [HttpPost("CreateTrophee")]
        public async Task<IActionResult> CreateTrophee([FromBody] CreateTropheeDto trophee)
        {
            if(trophee == null)
            {
                return BadRequest("Les données du trophée sont invalides.");
            }

            int tropheeId = await _tropheeBLL.CreateTrophee(trophee);
            return Ok(new { ID_Trophée = tropheeId, Message = "Trophée créé avec succès." });
        }

    //--------------------------GET ALL TROPHEES-------------------------------------------------------------------------------------------------------------

    [HttpGet("GetAllTrophee")]
    public async Task<IActionResult> GetAllTrophee()
    {
        try
        {
            var trophees = await _tropheeBLL.GetAllTrophees();
            return Ok(trophees);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    }
}
