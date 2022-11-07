using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PlacowkaOswiatowa.Domain.Models.Base;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Adres : BaseEntity<int>
    {
        public Adres()
        {
            AdresUczniowie = new HashSet<Uczen>();
            AdresPracownicyAdresy = new HashSet<PracownicyAdresy>();
        }

        public int PanstwoId { get; set; }

        [ForeignKey(nameof(PanstwoId))]
        [InverseProperty("PanstwoAdresy")]
        public virtual Panstwo Panstwo { get; set; }

        public int MiejscowoscId { get; set; }

        [ForeignKey(nameof(MiejscowoscId))]
        [InverseProperty("MiejscowoscAdresy")]
        public virtual Miejscowosc Miejscowosc { get; set; }

        public int? UlicaId { get; set; }

        [ForeignKey(nameof(UlicaId))]
        [InverseProperty("UlicaAdresy")]
        public virtual Ulica Ulica { get; set; }

        [MaxLength(10)]
        public string NumerDomu { get; set; }

        [MaxLength(10)]
        public string NumerMieszkania { get; set; }

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
            return PanstwoId ^ MiejscowoscId ^ UlicaId.GetValueOrDefault() ^ NumerDomu.GetHashCode() ^ NumerMieszkania.GetHashCode();
        }
    }
}
