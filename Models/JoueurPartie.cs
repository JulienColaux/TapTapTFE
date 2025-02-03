using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class JoueurPartie
    {
        public int ID_Joueur { get; set; }
        public string Nom { get; set; }
        public string Avatar_URL { get; set; }
        public int? Points { get; set; }
    }
}
