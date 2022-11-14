using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class OcenaZestawuPytan : BaseEntity<int>
    {
        public OcenaZestawuPytan()
        {
            OcenaZestawuPytanWyniki = new HashSet<Wynik>();
        }

        [MaxLength(1024)]
        public string OpisOceny { get; set; }

        public int ZestawPytanId { get; set; }

        [ForeignKey(nameof(ZestawPytanId))]
        [InverseProperty("ZestawPytanOceny")]
        public virtual ZestawPytan ZestawPytan { get; set; }

        public virtual ICollection<Wynik> OcenaZestawuPytanWyniki { get; set; }
    }
}
