using System;
using System.Collections.Generic;

namespace Lounasprojekti.Models
{
    public partial class Ravintola
    {
        public Ravintola()
        {
            Arvios = new HashSet<Arvio>();
            Lounastapahtumas = new HashSet<Lounastapahtuma>();
        }

        public int RavintolaId { get; set; }
        public string RavintolanNimi { get; set; } = null!;
        public string? Osoite { get; set; }
        public string? Postinumero { get; set; }
        public string? Postitoimipaikka { get; set; }
        public string? Verkkosivu { get; set; }
        public string? Kategoria { get; set; }

        public virtual ICollection<Arvio> Arvios { get; set; }
        public virtual ICollection<Lounastapahtuma> Lounastapahtumas { get; set; }
    }
}
