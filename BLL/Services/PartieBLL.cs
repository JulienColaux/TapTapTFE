﻿using DAL.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PartieBLL
    {
        //----------------------------INJECTION DE LA DE LA DAL-------------------------------------------------------------------


        private readonly PartieDAL _partieDAL;

        public PartieBLL(PartieDAL partieDAL)
        {
            _partieDAL = partieDAL;
        }

        //---------------------------------GET ALL INFO OF A PARTIE BY ID---------------------------------------------------------------


        public async Task<Partie> GetPartieById(int id)
        {
            if (id <= 0) throw new System.ArgumentException("ID invalide."); //ID doit ètre positive
            return await _partieDAL.GetPartieByID(id);
        }

        //---------------------------------GET ALL  PARTIEs---------------------------------------------------------------

        public async Task<List<Partie>> GetAllParties()
        {
            return await _partieDAL.GetAllParties();
        }


        //-------------------------------ADD PARTIE--------------------------------------------------------------------------

        public async Task<int> AddPartie(bool amical)
        {
            // Retourne l'ID de la partie créée
            return await _partieDAL.AddPartie(amical);
        }

        //-------------------------------ADD JOUE--------------------------------------------------------------------------
        public async Task AddJoue(int joueurId, int partieId, int points)
        {
            await _partieDAL.AddJoue(joueurId, partieId, points);
        }
    }
}
