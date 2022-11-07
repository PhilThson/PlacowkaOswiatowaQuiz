using PlacowkaOswiatowaQuiz.Data.Models.Base;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Etat : BaseDictionaryEntity<byte>
    {
        public Etat()
        {
            EtatPracownicy = new HashSet<Pracownik>();
        }

        public virtual ICollection<Pracownik> EtatPracownicy { get; set; }
    }
}
