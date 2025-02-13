using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")] // Route RESTful
    [ApiController]
    public class PartieController : Controller
    {
        private readonly PartieBLL _partieBLL;

        public PartieController(PartieBLL partieBLL)
        {
            _partieBLL = partieBLL;
        }

        // Récupérer une partie par ID
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

        // Récupérer toutes les parties
        [HttpGet("allParties")]
        public async Task<ActionResult<List<Partie>>> GetAllParties()
        {
            try
            {
                var parties = await _partieBLL.GetAllParties();
                return Ok(parties);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Ajouter une partie (Body au lieu de Query)
        [HttpPost]
        public async Task<ActionResult> AddGame([FromBody] PartieCreateDTO partieDTO)
        {
            try
            {
                int partieId = await _partieBLL.AddPartie(partieDTO.Amical);
                return Ok(new { id = partieId }); // Renvoie l'ID au client
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // Ajouter un joueur à une partie (Body + meilleure structuration)
        [HttpPost("{partieId}/joueur")]
        public async Task<ActionResult> AddJoue(int partieId, int JoueurId, int Points)
        {
            try
            {
                await _partieBLL.AddJoue(JoueurId, partieId, Points);
                return Ok(new { message = "Joueur ajouté à la partie avec succès." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // DTOs pour structurer les entrées POST
    public class PartieCreateDTO
    {
        public bool Amical { get; set; }
    }

    public class JoueCreateDTO
    {
        public int JoueurId { get; set; }
        public int Points { get; set; }
    }
}
