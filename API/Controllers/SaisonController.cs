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
    }
}
