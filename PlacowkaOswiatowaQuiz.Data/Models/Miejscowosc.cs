using PlacowkaOswiatowa.Domain.Models.Base;
using System.Collections.Generic;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Miejscowosc : BaseDictionaryEntity<int>
    {
        public Miejscowosc()
        {
            MiejscowoscAdresy = new HashSet<Adres>();
        }

        public virtual ICollection<Adres> MiejscowoscAdresy { get; set; }
    }
}
