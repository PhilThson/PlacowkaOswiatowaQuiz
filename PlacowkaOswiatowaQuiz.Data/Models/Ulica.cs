using PlacowkaOswiatowa.Domain.Models.Base;
using System.Collections.Generic;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Ulica : BaseDictionaryEntity<int>
    {
        public Ulica()
        {
            UlicaAdresy = new HashSet<Adres>();
        }

        public ICollection<Adres> UlicaAdresy { get; set; }
    }
}
