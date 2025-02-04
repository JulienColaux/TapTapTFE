﻿using BLL.Services;
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

        //--------------------------ADD JOUE-----------------------------------------------------------------------------


        [HttpPost("addSaison")]
        public async Task<ActionResult> AddSaison(int tropheeId)
        {
            try
            {
                await _saisonBLL.AddSaison(tropheeId);
                return Ok("Saison ajouter avec succé.");
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
