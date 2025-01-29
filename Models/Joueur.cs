using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Joueur
    {
        public int ID_Joueur { get; set; }
        public string Nom { get; set; }
        public string Avatar_URL { get; set; }
        public decimal XP { get; set; }
        public int? ID_EchelleGrade { get; set; }
        public int? Elo {  get; set; }
        public List<Trophee> Trophees { get; set; } = new List<Trophee>();
    }
}
