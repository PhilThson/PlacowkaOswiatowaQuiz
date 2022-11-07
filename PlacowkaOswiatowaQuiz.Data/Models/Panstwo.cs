using PlacowkaOswiatowa.Domain.Models.Base;
using System.Collections.Generic;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Panstwo : BaseDictionaryEntity<int>
    {
        public Panstwo()
        {
            PanstwoAdresy = new HashSet<Adres>();
        }

        public virtual ICollection<Adres> PanstwoAdresy { get; set; }
    }
}
