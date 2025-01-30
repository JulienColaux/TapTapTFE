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



        public async Task<string> GetUrlImageTropheeById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'ID de l'image doit être un entier positif.");
            }

            Console.WriteLine($"📢 Service : Récupération de l'URL de l'image avec ID = {id}");

            string url = await _tropheeDAL.GetUrlImageTropheeById(id);

            if (string.IsNullOrEmpty(url))
            {
                Console.WriteLine($"⚠️ Service : Aucune URL trouvée pour ID {id}");
                return null;
            }

            Console.WriteLine($"✅ Service : URL retournée = {url}");
            return url;
        }

    }
}
