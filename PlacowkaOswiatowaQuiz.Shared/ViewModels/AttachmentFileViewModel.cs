using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class AttachmentFileViewModel
	{
        public int Id { get; set; }
        [DisplayName("Nazwa pliku")]
        public string Name { get; set; }
        [DisplayName("Zawartość")]
        public byte[] Content { get; set; }
        [DisplayName("Rodzaj zawartości")]
        public string ContentType { get; set; }
        [DisplayName("Wielkość")]
        public long Size { get; set; }
    }
}