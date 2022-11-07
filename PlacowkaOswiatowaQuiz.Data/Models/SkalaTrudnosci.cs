using PlacowkaOswiatowaQuiz.Data.Models.Base;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class SkalaTrudnosci : BaseDictionaryEntity<byte>
    {
        public SkalaTrudnosci()
        {
            SkalaTrudnosciPytania = new HashSet<Pytanie>();
        }

        public virtual ICollection<Pytanie> SkalaTrudnosciPytania { get; set; }
    }
}
