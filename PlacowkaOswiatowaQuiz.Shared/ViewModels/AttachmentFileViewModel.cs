using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class AttachmentFileViewModel : AttachmentViewModel
	{
        [DisplayName("Zawartość")]
        public byte[] Content { get; set; }
    }
}