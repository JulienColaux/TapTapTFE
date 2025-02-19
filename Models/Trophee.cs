﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Trophee
    {
        public int ID_Trophée { get; set; }
        public string Nom { get; set; }
        public DateTime Date_Acquisition { get; set; }
        public int? ID_Joueur { get; set; }
        public string Url_image { get; set; }
    }

    public class TropheeForGetAll
    {
        public int ID_Trophée { get; set; }
        public int ID_Saison { get; set; }
        public string Nom { get; set; }
        public DateTime Date_Acquisition { get; set; }
        public int? ID_Joueur { get; set; }
        public string Url_image { get; set; }

    }
}
