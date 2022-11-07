using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Przedmiot : BaseDictionaryEntity<byte>
    {
        public Przedmiot()
        {
            PrzedmiotPrzedmiotyPracownicy = new HashSet<PrzedmiotyPracownicy>();
        }

        [Column(TypeName = "char(3)")]
        public string SkroconaNazwa { get; set; }

        public virtual ICollection<PrzedmiotyPracownicy> PrzedmiotPrzedmiotyPracownicy 
        { get; set; }
    }
}
