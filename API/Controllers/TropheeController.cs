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

    }
}
