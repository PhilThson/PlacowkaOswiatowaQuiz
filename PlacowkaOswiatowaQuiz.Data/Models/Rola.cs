using PlacowkaOswiatowa.Domain.Models.Base;
using System.Collections.Generic;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Rola : BaseDictionaryEntity<byte>
    {
        public Rola()
        {
            RolaUzytkownicy = new HashSet<Uzytkownik>();
        }

        public virtual ICollection<Uzytkownik> RolaUzytkownicy { get; set; }
    }
}
