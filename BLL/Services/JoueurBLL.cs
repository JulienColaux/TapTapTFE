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
            if (id <= 0) throw new System.ArgumentException("ID invalide."); //ID doit ètre positive
            return await _joueurDAL.GetJoueurById(id);
        }



        //-----------------------------GET CLASSEMENT--------------------------------------------------------------------------



        public async Task<List<Joueur>> GetClassement()
        {
            return await _joueurDAL.GetClassement();
        }


        //------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<Trophee>> GetAllTropheesByJoueurId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'ID du joueur doit être un entier positif.");
            }

            return  _joueurDAL.GetAllTropheesByJoueurId(id);
        }

    }
}
