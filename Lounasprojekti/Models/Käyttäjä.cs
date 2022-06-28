using System;
using System.Collections.Generic;

namespace Lounasprojekti.Models
{
    public partial class Käyttäjä
    {
        public Käyttäjä()
        {
            Arvios = new HashSet<Arvio>();
            Lounasseuras = new HashSet<Lounasseura>();
        }

        public int KäyttäjäId { get; set; }
        public string Käyttäjänimi { get; set; } = null!;
        public int? OnAdmin { get; set; }

        public virtual ICollection<Arvio> Arvios { get; set; }
        public virtual ICollection<Lounasseura> Lounasseuras { get; set; }
    }
}
