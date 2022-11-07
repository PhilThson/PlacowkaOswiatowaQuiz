using PlacowkaOswiatowaQuiz.Data.Models.Base;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Stanowisko : BaseDictionaryEntity<byte>
    {
        public Stanowisko()
        {
            StanowiskoPracownicy = new HashSet<Pracownik>();
        }

        public virtual ICollection<Pracownik> StanowiskoPracownicy { get; set; }
    }
}
