using PlacowkaOswiatowa.Domain.Models.Base;
using System.Collections.Generic;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Etat : BaseDictionaryEntity<byte>
    {
        public Etat()
        {
            EtatPracownicy = new HashSet<Pracownik>();
        }

        public virtual ICollection<Pracownik> EtatPracownicy { get; set; }
    }
}
