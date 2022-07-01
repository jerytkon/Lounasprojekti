using System;
using System.Collections.Generic;

namespace LounasprojektiLib.Models
{
    public partial class Lounastapahtuma
    {
        public Lounastapahtuma()
        {
            Lounasseuras = new HashSet<Lounasseura>();
        }

        public int LounastapahtumaId { get; set; }
        public int? RavintolaId { get; set; }
        public DateTime Päivämäärä { get; set; }

        public virtual Ravintola? Ravintola { get; set; }
        public virtual ICollection<Lounasseura> Lounasseuras { get; set; }
    }
}
