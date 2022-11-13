using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class ObszarZestawuPytan : BaseDictionaryEntity<byte>
    {
        public ObszarZestawuPytan()
        {
            ObszarZestawuPytanZestawyPytan = new HashSet<ZestawPytan>();
        }

        [MaxLength(1024)]
        public string? NazwaRozszerzona { get; set; }

        public virtual ICollection<ZestawPytan> ObszarZestawuPytanZestawyPytan { get; set; }
    }
}
