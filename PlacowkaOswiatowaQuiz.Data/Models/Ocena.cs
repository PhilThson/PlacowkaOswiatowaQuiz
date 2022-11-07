using PlacowkaOswiatowa.Domain.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Ocena : BaseEntity<long>
    {
        [Range(1.0, 6.0)]
        public decimal WystawionaOcena { get; set; }

        public int UczenId { get; set; }

        [ForeignKey(nameof(UczenId))]
        [InverseProperty("UczenOceny")]
        public virtual Uczen Uczen { get; set; }

        public byte PrzedmiotId { get; set; }

        [ForeignKey(nameof(PrzedmiotId))]
        public virtual Przedmiot Przedmiot { get; set; }

        public int PracownikId { get; set; }

        [ForeignKey(nameof(PracownikId))]
        [InverseProperty("PracownikOceny")]
        public virtual Pracownik Pracownik { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataWystawienia { get; set; }

        [Range(1.0, 6.0)]
        public decimal? PoprzedniaOcena { get; set; }

        public DateTime? DataPoprawienia { get; set; }
    }
}
