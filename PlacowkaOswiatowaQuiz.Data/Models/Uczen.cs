using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Uczen : Osoba
    {
        public Uczen()
        {
            UczenOceny = new HashSet<Ocena>();
        }

        public int WychowawcaId { get; set; }

        [Required]
        [ForeignKey(nameof(WychowawcaId))]
        [InverseProperty("PracownikUczniowie")]
        public virtual Pracownik Wychowawca { get; set; }

        public byte OddzialId { get; set; }

        [Required]
        [ForeignKey(nameof(OddzialId))]
        [InverseProperty("OddzialUczniowie")]
        public virtual Oddzial Oddzial { get; set; }

        public int? AdresId { get; set; }

        [ForeignKey(nameof(AdresId))]
        [InverseProperty("AdresUczniowie")]
        public virtual Adres Adres { get; set; }

        public virtual ICollection<Ocena> UczenOceny { get; set; }
    }
}
