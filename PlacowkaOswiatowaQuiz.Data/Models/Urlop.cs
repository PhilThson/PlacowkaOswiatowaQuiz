using PlacowkaOswiatowa.Domain.Models.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Urlop
    {
        public int PracownikId { get; set; }

        [ForeignKey(nameof(PracownikId))]
        [InverseProperty("PracownikUrlopy")]
        public virtual Pracownik Pracownik { get; set; }

        [Column(TypeName = "date")]
        public DateTime PoczatekUrlopu { get; set; }

        [Column(TypeName = "date")]
        public DateTime KoniecUrlopu { get; set; }

        public bool CzyAktywny { get; set; }
    }
}
