using PlacowkaOswiatowa.Domain.Models.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Migawka : BaseDictionaryEntity<long>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataZmiany { get; set; }
        public string Szczegoly { get; set; }
        public int UzytkownikId { get; set; }

        [ForeignKey(nameof(UzytkownikId))]
        [InverseProperty("UzytkownikMigawki")]
        public virtual Uzytkownik Uzytkownik { get; set; }
    }
}
