using PlacowkaOswiatowa.Domain.Models.Base;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowa.Domain.Models
{
    /// <summary>
    /// Klasa opisująca użytkownika korzystającego z aplikacji
    /// </summary>
    public class Uzytkownik : BaseEntity<int>
    {
        public Uzytkownik()
        {
            UzytkownikMigawki = new HashSet<Migawka>();
        }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Imie { get; set; }

        [MaxLength(50)]
        public string Nazwisko { get; set; }

        [MaxLength(200)]
        public string HashHasla { get; set; }

        public byte RolaId { get; set; }

        [ForeignKey(nameof(RolaId))]
        [InverseProperty("RolaUzytkownicy")]
        public virtual Rola Rola { get; set; }

        public virtual ICollection<Migawka> UzytkownikMigawki { get; set; }
    }
}
