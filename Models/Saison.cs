using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Saison
    {
        public int ID_Saison {  get; set; }
        public DateTime Decompte {  get; set; }
        public int? ID_Trophée { get; set; } = null;
    }
}
