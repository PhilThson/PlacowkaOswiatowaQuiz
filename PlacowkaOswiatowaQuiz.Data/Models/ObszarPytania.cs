using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class ObszarPytania : BaseDictionaryEntity<byte>
    {
        public ObszarPytania()
        {
            ObszarPytaniePytania = new HashSet<Pytanie>();
        }

        [MaxLength(128)]
        public string NazwaRozszerzona { get; set; }

        public virtual ICollection<Pytanie> ObszarPytaniePytania { get; set; }
    }
}
