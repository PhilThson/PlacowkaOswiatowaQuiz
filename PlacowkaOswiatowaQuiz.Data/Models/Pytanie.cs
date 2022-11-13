using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PlacowkaOswiatowaQuiz.Data.Models.Base;

namespace PlacowkaOswiatowaQuiz.Data.Models
{
	public class Pytanie : BaseEntity<int>
	{
        [MaxLength(2048)]
        public string Tresc { get; set; }

		[MaxLength(2048)]
		public string Opis { get; set; }

		public int ZestawPytanId { get; set; }

		[ForeignKey(nameof(ZestawPytanId))]
		[InverseProperty("ZestawPytanPytania")]
		public virtual ZestawPytan ZestawPytan { get; set; }
	}
}

