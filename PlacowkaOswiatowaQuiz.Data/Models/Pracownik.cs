using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacowkaOswiatowa.Domain.Models
{
    public class Pracownik : Osoba
    {
        public Pracownik()
        {
            PracownikUczniowie = new HashSet<Uczen>();
            PracownikUrlopy = new HashSet<Urlop>();
            PracownikOceny = new HashSet<Ocena>();
            PracownikPracownicyAdresy = new HashSet<PracownicyAdresy>();
            PracownikPrzedmiotyPracownicy = new HashSet<PrzedmiotyPracownicy>();
        }

        [Range(2800.0, 10000.0)]
        public decimal Pensja { get; set; }

        [Range(0, 500)]
        public int? DniUrlopu { get; set; }

        public double? WymiarGodzinowy { get; set; }

        public double? Nadgodziny { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string NrTelefonu { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public byte EtatId { get; set; }

        [ForeignKey(nameof(EtatId))]
        [InverseProperty("EtatPracownicy")]
        public virtual Etat Etat { get; set; }

        public byte StanowiskoId { get; set; }

        [ForeignKey(nameof(StanowiskoId))]
        [InverseProperty("StanowiskoPracownicy")]
        public virtual Stanowisko Stanowisko { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataZatrudnienia { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataKoncaZatrudnienia { get; set; }

        public virtual Oddzial PracownikOddzial { get; set; }
        public virtual ICollection<Uczen> PracownikUczniowie { get; set; }
        public virtual ICollection<Urlop> PracownikUrlopy { get; set; }
        public virtual ICollection<Ocena> PracownikOceny { get; set; }
        public virtual ICollection<PracownicyAdresy> PracownikPracownicyAdresy 
        { get; set; }
        public virtual ICollection<PrzedmiotyPracownicy> PracownikPrzedmiotyPracownicy 
        { get; set; }
    }
}
