using System;
using Microsoft.AspNetCore.Http;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class AttachmentFileViewModel
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
    }
}