using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Migawka : BaseDictionaryEntity<long>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataZmiany { get; set; }
        public string? Szczegoly { get; set; }
        public int UzytkownikId { get; set; }

        [ForeignKey(nameof(UzytkownikId))]
        [InverseProperty("UzytkownikMigawki")]
        public virtual Uzytkownik Uzytkownik { get; set; }
    }
}
