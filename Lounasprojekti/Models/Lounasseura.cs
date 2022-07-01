using System;
using System.Collections.Generic;

namespace LounasprojektiLib.Models
{
    public partial class Lounasseura
    {
        public int SeuraId { get; set; }
        public int? LounastapahtumaId { get; set; }
        public int? KäyttäjäId { get; set; }

        public virtual Käyttäjä? Käyttäjä { get; set; }
        public virtual Lounastapahtuma? Lounastapahtuma { get; set; }
    }
}
