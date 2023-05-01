using System;
using System.ComponentModel;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class AttachmentViewModel
	{
        public int Id { get; set; }
        [DisplayName("Nazwa pliku")]
        public string Name { get; set; }
        [DisplayName("Opis")]
        public string? Description { get; set; }
        [DisplayName("Rodzaj zawartości")]
        public string? ContentType { get; set; }
        [DisplayName("Wielkość")]
        public long Size { get; set; }
        [DisplayName("Id zestawu pytań")]
        public int QuestionsSetId { get; set; }
    }
}

