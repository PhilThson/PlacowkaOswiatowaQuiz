using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Odpowiedz : BaseEntity<int>
    {
        public Odpowiedz()
        {
            OdpowiedzWyniki = new HashSet<Wynik>();
        }

        [MaxLength(1024)]
        public string OdpowiedzTestowa { get; set; }

        [MaxLength(1024)]
        public string OdpowiedzRaportowana { get; set; }

        public int PytanieId { get; set; }

        [ForeignKey(nameof(PytanieId))]
        [InverseProperty("PytanieOdpowiedzi")]
        public virtual Pytanie Pytanie { get; set; }

        public virtual ICollection<Wynik> OdpowiedzWyniki { get; set; }
    }
}
