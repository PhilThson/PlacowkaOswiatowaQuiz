using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Wynik : BaseEntity<long>
    {
        public int UczenId { get; set; }

        [ForeignKey(nameof(UczenId))]
        [InverseProperty("UczenWyniki")]
        public virtual Uczen Uczen { get; set; }

        public int PytanieId { get; set; }

        [ForeignKey(nameof(PytanieId))]
        [InverseProperty("PytanieWyniki")]
        public virtual Pytanie Pytanie { get; set; }

        public int OdpowiedzId { get; set; }

        [ForeignKey(nameof(OdpowiedzId))]
        [InverseProperty("OdpowiedzWyniki")]
        public virtual Odpowiedz Odpowiedz { get; set; }

        public int PracownikId { get; set; }

        [ForeignKey(nameof(PracownikId))]
        [InverseProperty("PracownikWyniki")]
        public virtual Pracownik Pracownik { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataCzasWpisu { get; set; }
    }
}
