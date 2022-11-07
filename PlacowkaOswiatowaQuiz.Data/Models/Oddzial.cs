using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PlacowkaOswiatowa.Domain.Models.Base;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Oddzial : BaseDictionaryEntity<byte>
    {
        public Oddzial()
        {
            OddzialUczniowie = new HashSet<Uczen>();
        }

        public int PracownikId { get; set; }

        [ForeignKey(nameof(PracownikId))]
        [InverseProperty("PracownikOddzial")]
        public virtual Pracownik Pracownik { get; set; }

        public ICollection<Uczen> OddzialUczniowie { get; set; }
    }
}
