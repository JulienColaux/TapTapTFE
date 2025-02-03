using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{

    [Route("api/[controller]")] // La route
    [ApiController]
    public class GradeController : Controller
    {
        //---------------------INJECTION BLL-------------------------------------------------------------------------------------


        private readonly GradeBLL _gradeBLL;

        public GradeController(GradeBLL gradeBLL)
        {
            _gradeBLL = gradeBLL;
        }

        //----------------------GET GRADE BY ID---------------------------------------------------------------------------


        [HttpGet("{id}")]  //Méthode get avec un paramètre url id

        public async Task<ActionResult<Grade>> GetGradeById(int id)  //Action result permet de renvoyer des objet ou des code http comme 404 not found par exemple
        {
            try
            {
                var grade = await _gradeBLL.GetTropheeById(id);
                return Ok(grade);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });//ce format renvoie en json alors que juste badRequest(ex.Message) juste string
            }
        }


        //-------------------------------GET ALL GRADE--------------------------------------------------------------------------------------------------------

        [HttpGet("allGrade")]  

        public async Task<ActionResult<List<Grade>>> GetAllGrade()  //Action result permet de renvoyer des objet ou des code http comme 404 not found par exemple
        {
            try
            {
                var grade = await _gradeBLL.GetAllGrades();
                return Ok(grade);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });//ce format renvoie en json alors que juste badRequest(ex.Message) juste string
            }
        }

    }
}
