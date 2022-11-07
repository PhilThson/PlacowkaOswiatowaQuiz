using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class PracownicyAdresy
    {
        public int AdresId { get; set; }

        [ForeignKey(nameof(AdresId))]
        [InverseProperty("AdresPracownicyAdresy")]
        public virtual Adres Adres { get; set; }

        public int PracownikId { get; set; }

        [ForeignKey(nameof(PracownikId))]
        [InverseProperty("PracownikPracownicyAdresy")]
        public virtual Pracownik Pracownik { get; set; }
    }
}
