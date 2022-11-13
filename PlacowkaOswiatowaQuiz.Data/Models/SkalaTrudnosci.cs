using PlacowkaOswiatowaQuiz.Data.Models.Base;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class SkalaTrudnosci : BaseDictionaryEntity<byte>
    {
        public SkalaTrudnosci()
        {
            SkalaTrudnosciZestawyPytan = new HashSet<ZestawPytan>();
        }

        public virtual ICollection<ZestawPytan> SkalaTrudnosciZestawyPytan
        { get; set; }
    }
}
