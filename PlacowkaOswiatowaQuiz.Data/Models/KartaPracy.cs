using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PlacowkaOswiatowaQuiz.Data.Models.Base;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
	public class KartaPracy : BaseDictionaryEntity<int>
	{
		public byte[] Zawartosc { get; set; }

        [MaxLength(64)]
        public string RodzajZawartosci { get; set; }

        public long Rozmiar { get; set; }

		[InverseProperty("KartaPracy")]
		public virtual ZestawPytan KartaPracyZestawPytan { get; set; }
	}
}