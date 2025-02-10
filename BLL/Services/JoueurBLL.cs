using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DAL.Repositories;

namespace BLL.Services
{
    public class JoueurBLL
    {

        //----------------------------INJECTION DE LA DE LA DAL-------------------------------------------------------------------


        private readonly JoueurDAL _joueurDAL;

        public JoueurBLL(JoueurDAL joueurDAL)
        {
            _joueurDAL = joueurDAL;
        }


        //---------------------------GET JOUEUR ALL INFO----------------------------------------------------------------------


        public async Task<Joueur> GetJoueurDetails(int id)
        {
            if (id <= 0)
            {
                throw new System.ArgumentException("ID invalide."); //ID doit ètre positive
            }
            return await _joueurDAL.GetJoueurById(id);
        }



        //-----------------------------ADD POINT--------------------------------------------------------------------------

        public async Task AddPoints(int joueurId, int seasonId, int points)
        {
            await _joueurDAL.AddPoints(joueurId, seasonId, points);
        }


        //-----------------------------ADD XP--------------------------------------------------------------------------

        public async Task AddXP(int joueurId, int xp)
        {
            await _joueurDAL.AddXP(joueurId, xp);
        }



        //-----------------------------GET ALL TROPHEE D UN JOUEUR------------------------------------------------------

        public async Task<List<Trophee>> GetAllTropheesByJoueurId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'ID du joueur doit être un entier positif.");
            }

            return await _joueurDAL.GetAllTropheesByJoueurId(id); // Ici, on doit bien utiliser "await"
        }

        //-----------------------------------RESET SEASON POINT OF JOUEUR---------------------------------------------------------------

        public async Task ResetPointsById(int joueurId)
        {
            if (joueurId <= 0)
            {
                throw new ArgumentException("L'ID du joueur doit être un entier positif.");
            }

            await _joueurDAL.ResetPointsById(joueurId); 
        }


    }
}
