using PlacowkaOswiatowaQuiz.Data.Models.Base;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Rola : BaseDictionaryEntity<byte>
    {
        public Rola()
        {
            RolaUzytkownicy = new HashSet<Uzytkownik>();
        }

        public virtual ICollection<Uzytkownik> RolaUzytkownicy { get; set; }
    }
}
