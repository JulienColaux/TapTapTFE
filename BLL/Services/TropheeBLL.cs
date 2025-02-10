using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using Models;

namespace BLL.Services
{
    public class TropheeBLL
    {

        //----------------------------INJECTION DE LA DE LA DAL-------------------------------------------------------------------


        private readonly TropheeDAL _tropheeDAL;

        public TropheeBLL(TropheeDAL tropheeDAL)
        {
            _tropheeDAL = tropheeDAL;
        }


        //----------------------------GET TROPHEE BY ID-----------------------------------------------------------------------------


        public async Task<Trophee>GetTropheeById(int id)
        {
            if (id <= 0) throw new System.ArgumentException("ID invalide");
            return await _tropheeDAL.GetTropheeById(id);
        }


        //---------------------------GET IMAGE URL TROPHEE BY ID-------------------------------------------------------------------


        public async Task<string> GetUrlImageTropheeByTropheeId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'ID de l'image doit être un entier positif.");
            }


            string url = await _tropheeDAL.GetUrlImageTropheeByTropheeId(id);

            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            Console.WriteLine($" Service : URL retournée = {url}");
            return url;
        }

        //--------------------------ADD TROPHEE TO JOUEUR---------------------------------------------------------------------------------------------------

        public async Task<int> CreateTrophee(CreateTropheeDto trophee)
        {
            return await _tropheeDAL.CreateTrophee(trophee);
        }

    }
}
