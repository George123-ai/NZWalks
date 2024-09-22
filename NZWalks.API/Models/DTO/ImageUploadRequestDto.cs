using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
	public class ImageUploadRequestDto
	{
		[Required]
		public IFormFile File { get; set; }

		public string FileName { get; set; }
		public string? FileDescription { get; set; }
		



	}
}
