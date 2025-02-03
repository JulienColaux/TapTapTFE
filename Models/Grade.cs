using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Grade
    {
        public int ID_EchelleGrade { get; set; }
        public string Nom { get; set; }
        public int? ValeurMinimal { get; set; }
    }
}