using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly IMapper mapper;
		private readonly IImageRepository imageRepository;

		public ImagesController(IMapper mapper,IImageRepository imageRepository)
		{
			this.mapper = mapper;
			this.imageRepository = imageRepository;
		}


		[HttpPost] 
		[Route("Upload")]
		public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
		{
			ValidateFileUpload(request);

			if (ModelState.IsValid)
			{
				//var imageDomainModel = mapper.Map<Image>(request);
				var imageDomainModel = new Image
				{
					File = request.File,
					FileExtension = Path.GetExtension(request.File.FileName),
					FileSizeInBytes = request.File.Length,
					FileName = request.FileName,
					FileDescription = request.FileDescription
				};

				// user repo to upload image
				await imageRepository.Upload(imageDomainModel);

				return Ok(imageDomainModel);
			}

			return BadRequest(ModelState);	
		}

		private void ValidateFileUpload(ImageUploadRequestDto request)
		{
			var allowedExtensions = new string[]
			{
				".jpg", ".jped", ".png"
			};

			if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
			{
				ModelState.AddModelError("file", "Unsupported file extension");
			}

			if (request.File.Length > 10485760)
			{
				ModelState.AddModelError("file", "File size more than 10MB");
			}
		}
	}
}
