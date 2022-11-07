using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Pytanie : BaseEntity<int>
    {
        public Pytanie()
        {
            PytanieOdpowiedzi = new HashSet<Odpowiedz>();
            PytanieWyniki = new HashSet<Wynik>();
        }

        [MaxLength(1024)]
        public string Tresc { get; set; }

        public byte ObszarPytaniaId { get; set; }

        [ForeignKey(nameof(ObszarPytaniaId))]
        [InverseProperty("ObszarPytaniePytania")]
        public virtual ObszarPytania ObszarPytania { get; set; }

        public byte SkalaTrudnosciId { get; set; }

        [ForeignKey(nameof(SkalaTrudnosciId))]
        [InverseProperty("SkalaTrudnosciPytania")]
        public virtual SkalaTrudnosci SkalaTrudnosci { get; set; }

        public virtual ICollection<Odpowiedz> PytanieOdpowiedzi { get; set; }

        public virtual ICollection<Wynik> PytanieWyniki { get; set; }
    }
}
