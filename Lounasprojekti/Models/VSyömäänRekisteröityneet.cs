﻿using System;
using System.Collections.Generic;

namespace Lounasprojekti.Models
{
    public partial class VSyömäänRekisteröityneet
    {
        public string RavintolanNimi { get; set; } = null!;
        public int? SyömäänTulijat { get; set; }
        public DateTime? Päivämäärä { get; set; }
    }
}
