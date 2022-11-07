using PlacowkaOswiatowa.Domain.Models.Base;
using System.Collections.Generic;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Stanowisko : BaseDictionaryEntity<byte>
    {
        public Stanowisko()
        {
            StanowiskoPracownicy = new HashSet<Pracownik>();
        }

        public virtual ICollection<Pracownik> StanowiskoPracownicy { get; set; }
    }
}
