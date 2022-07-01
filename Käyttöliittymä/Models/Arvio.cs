using System;
using System.Collections.Generic;

namespace Käyttöliittymä.Models
{
    public partial class Arvio
    {
        public int ArvioId { get; set; }
        public int? RavintolaId { get; set; }
        public int? KäyttäjäId { get; set; }
        public DateTime? Päivämäärä { get; set; }
        public int? Arvosana { get; set; }
        public string? Kommentti { get; set; }

        public virtual Käyttäjä? Käyttäjä { get; set; }
        public virtual Ravintola? Ravintola { get; set; }
    }
}
