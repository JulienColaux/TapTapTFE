using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Partie
    {
        public int ID_Partie {  get; set; }
        public DateTime Date_Partie { get; set; }
        public Boolean Amical {  get; set; } 
 
        public List<JoueurPartie> Joueurs { get; set; }
    }
}
