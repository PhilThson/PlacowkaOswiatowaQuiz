using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PlacowkaOswiatowa.Domain.Models.Base;

namespace PlacowkaOswiatowa.Domain.Models
{
    public abstract class Osoba : BaseEntity<int>
    {
        [Required]
        [MaxLength(20)]
        public string Imie { get; set; }

        [MaxLength(20)]
        public string DrugieImie { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nazwisko { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "date")]
        public DateTime? DataUrodzenia { get; set; }

        [Required]
        [MaxLength(11)]
        [Column(TypeName = "varchar(11)")]
        public string Pesel { get; set; }

        public static bool operator ==(Osoba o1, Osoba o2)
        {
            return o1.GetHashCode() == o2.GetHashCode();
        }
        public static bool operator !=(Osoba o1, Osoba o2)
        {
            return !(o1 == o2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Osoba)) return false;
            Osoba o = (Osoba)obj;
            return (this == o);
        }
        public override int GetHashCode()
        {
            return Imie.GetHashCode() ^ Nazwisko.GetHashCode() ^ Pesel.GetHashCode();
        }
    }
}
