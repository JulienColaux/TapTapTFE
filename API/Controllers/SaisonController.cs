using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")] // La route
    [ApiController]
    public class SaisonController : Controller
    {
        //---------------------INJECTION BLL-------------------------------------------------------------------------------------


        private readonly  SaisonBLL _saisonBLL;

        public SaisonController(SaisonBLL saisonBLL)
        {
            _saisonBLL = saisonBLL;
        }


        //---------------------GET SAISON BY ID---------------------------------------------------------------------------------


        [HttpGet("{id}")]

        public async Task<ActionResult<Saison>> GetSaisonById(int id)
        {
            try
            {
                var saison = await _saisonBLL.GetSaisonById(id);
                return Ok(saison);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message =  ex.Message});
            }
        }


        //-----------------------GET CLASSEMENT-----------------------------------------------------------------------------

        [HttpGet("{saisonId}/classement")]

        public async Task<ActionResult<List<JoueurPartie>>>GetClassement(int saisonId)
        {
            try
            {
                var classement = await _saisonBLL.GetClassement(saisonId);
                return Ok(classement);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }


        //--------------------------ADD Saison-----------------------------------------------------------------------------

        [HttpPost("addSaison")]
        public async Task<ActionResult> AddSaison()
        {
            try
            {
                int saisonId = await _saisonBLL.AddSaison();
                return Ok(new { message = "Saison ajoutée avec succès.", saisonId = saisonId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        //--------------------------ADD Participe-----------------------------------------------------------------------------


        [HttpPost("addParticipe")]
        public async Task<ActionResult> AddParticipe(int joueurId, int saisonId, int points)
        {
            try
            {
                await _saisonBLL.AddJoue(joueurId, saisonId, points);
                return Ok("joueur ajouter a une saison ainsi que ces points");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
