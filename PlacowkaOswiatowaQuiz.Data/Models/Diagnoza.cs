using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PlacowkaOswiatowaQuiz.Data.Models.Base;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
	public class Diagnoza : BaseEntity<int>
	{
		public Diagnoza()
		{
            DiagnozaWyniki = new HashSet<Wynik>();
		}

		[Required]
		[MaxLength(9)]
		public string RokSzkolny { get; set; }

        public int UczenId { get; set; }

        [ForeignKey(nameof(UczenId))]
        [InverseProperty("UczenDiagnozy")]
        public virtual Uczen Uczen { get; set; }

        public int PracownikId { get; set; }

        [ForeignKey(nameof(PracownikId))]
        [InverseProperty("PracownikDiagnozy")]
        public virtual Pracownik Pracownik { get; set; }

        //Składany z opisów?
        //public string RozwojEmocjonalnoSpoleczny { get; set; }

        public virtual ICollection<Wynik> DiagnozaWyniki { get; set; }
    }
}

