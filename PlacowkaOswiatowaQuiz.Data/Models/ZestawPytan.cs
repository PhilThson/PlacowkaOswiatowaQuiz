using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class ZestawPytan : BaseEntity<int>
    {
        public ZestawPytan()
        {
            ZestawPytanPytania = new HashSet<Pytanie>();
            ZestawPytanOceny = new HashSet<OcenaZestawuPytan>();
        }

        [MaxLength(2048)]
        public string OpisUmiejetnosci { get; set; }

        public byte ObszarZestawuPytanId { get; set; }

        [ForeignKey(nameof(ObszarZestawuPytanId))]
        [InverseProperty("ObszarZestawuPytanZestawyPytan")]
        public virtual ObszarZestawuPytan ObszarZestawuPytan { get; set; }

        public byte SkalaTrudnosciId { get; set; }

        [ForeignKey(nameof(SkalaTrudnosciId))]
        [InverseProperty("SkalaTrudnosciZestawyPytan")]
        public virtual SkalaTrudnosci SkalaTrudnosci { get; set; }

        public int? KartaPracyId { get; set; }

        [ForeignKey(nameof(KartaPracyId))]
        [InverseProperty("KartaPracyZestawPytan")]
        public virtual KartaPracy KartaPracy { get; set; }

        public virtual ICollection<Pytanie> ZestawPytanPytania { get; set; }

        public virtual ICollection<OcenaZestawuPytan> ZestawPytanOceny
        { get; set; }
    }
}
