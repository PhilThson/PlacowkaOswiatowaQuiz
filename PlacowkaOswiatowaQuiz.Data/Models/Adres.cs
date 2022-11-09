using PlacowkaOswiatowaQuiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
    public class Adres : BaseEntity<int>
    {
        public Adres()
        {
            AdresUczniowie = new HashSet<Uczen>();
            AdresPracownicyAdresy = new HashSet<PracownicyAdresy>();
        }

        [MaxLength(64)]
        public string Panstwo { get; set; }

        [MaxLength(128)]
        public string Miejscowosc { get; set; }

        [MaxLength(128)]
        public string? Ulica { get; set; }

        [MaxLength(10)]
        public string NumerDomu { get; set; }

        [MaxLength(10)]
        public string? NumerMieszkania { get; set; }

        [MaxLength(10)]
        public string KodPocztowy { get; set; }

        public virtual ICollection<Uczen> AdresUczniowie { get; set; }
        public virtual ICollection<PracownicyAdresy> AdresPracownicyAdresy 
        { get; set; }

        public static bool operator ==(Adres a1, Adres a2)
        {
            return a1.GetHashCode() == a2.GetHashCode();
        }
        public static bool operator !=(Adres a1, Adres a2)
        {
            return !(a1 == a2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Adres)) return false;
            Adres a = (Adres)obj;
            return (this == a);
        }
        public override int GetHashCode()
        {
            return Panstwo.GetHashCode() ^ Miejscowosc.GetHashCode() ^ NumerDomu.GetHashCode()
                ^ KodPocztowy.GetHashCode();
        }
    }
}
