using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using Models;

namespace BLL.Services
{
    public class SaisonBLL
    {
        //----------------------------INJECTION DE LA DE LA DAL-------------------------------------------------------------------


        private readonly SaisonDAL _saisonDAL;

        public SaisonBLL(SaisonDAL saisonDAL)
        {
            _saisonDAL = saisonDAL;
        }


        //----------------------------GET SAISON BY ID----------------------------------------------------------------------------------


        public async Task<Saison>GetSaisonById(int id)
        {
            if (id <= 0) throw new Exception("ID invalide");
            return await _saisonDAL.GetSaisonById(id);
        }


        //-------------------------------ADD SAISON--------------------------------------------------------------------------

        public async Task AddSaison(int tropheeId )
        {
            await _saisonDAL.AddSaison(tropheeId);
        }

        //-------------------------------ADD PARTICIPE--------------------------------------------------------------------------
        public async Task AddJoue(int joueurId, int saisonId, int points)
        {
            await _saisonDAL.AddParticipe(joueurId, saisonId, points);
        }

        //------------------------------GET CLASSEMENT----------------------------------------------------------------------


        public async Task<List<JoueurPartie>> GetClassement(int saisonId)
        {
            if (saisonId <= 0) throw new Exception("ID invalide");
            return await _saisonDAL.GetClassement(saisonId);
        }
    }
}
