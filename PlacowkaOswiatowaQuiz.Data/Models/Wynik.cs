using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Wynik : BaseEntity<long>
    {
        public int OcenaZestawuPytanId { get; set; }

        [ForeignKey(nameof(OcenaZestawuPytanId))]
        [InverseProperty("OcenaZestawuPytanWyniki")]
        public virtual OcenaZestawuPytan OcenaZestawuPytan { get; set; }

        [Range(1, 6)]
        public byte PoziomOceny { get; set; }

        public int DiagnozaId { get; set; }

        [ForeignKey(nameof(DiagnozaId))]
        [InverseProperty("DiagnozaWyniki")]
        public virtual Diagnoza Diagnoza { get; set; }

        [MaxLength(2048)]
        public string Notatki { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataCzasWpisu { get; set; }
    }
}
