using System.ComponentModel.DataAnnotations.Schema;
using PlacowkaOswiatowaQuiz.Data.Models.Base;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Oddzial : BaseDictionaryEntity<byte>
    {
        public Oddzial()
        {
            OddzialUczniowie = new HashSet<Uczen>();
        }

        public int PracownikId { get; set; }

        [ForeignKey(nameof(PracownikId))]
        [InverseProperty("PracownikOddzial")]
        public virtual Pracownik Pracownik { get; set; }

        public ICollection<Uczen> OddzialUczniowie { get; set; }
    }
}
