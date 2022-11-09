using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Data.Models.Base
{
    public class BaseDictionaryEntity<T> : BaseEntity<T>
    {
        [Required]
        [MaxLength(50)]
        public string Nazwa { get; set; }

        [MaxLength(250)]
        public string? Opis { get; set; }
    }
}
