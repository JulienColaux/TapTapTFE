using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{

        public class CreateTropheeDto
        {
            public string Nom { get; set; }
            public int ID_Joueur { get; set; }
            public int ID_Saison { get; set; }
        }

}
