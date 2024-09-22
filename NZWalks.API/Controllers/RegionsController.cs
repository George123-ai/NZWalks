using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class RegionsController : ControllerBase
	{
		private readonly IRegionRepository regionRepository;
		private readonly IMapper mapper;
		private readonly ILogger<RegionsController> logger;

		//private readonly NZWalksDbContext nZWalksDbContext;

		public RegionsController(IRegionRepository regionRepository,
			IMapper mapper, ILogger<RegionsController> logger)//, NZWalksDbContext nZWalksDbContext)
		{
			this.regionRepository = regionRepository;
			this.mapper = mapper;
			this.logger = logger;
			this.logger = logger;
			//this.nZWalksDbContext = nZWalksDbContext;
		}


		[HttpGet]
		//[Authorize(Roles = "Reader")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				//throw new Exception("This is custom exception");

				//get data from database - domain models
				var regionsDomain = await regionRepository.GetAllAsync();

				//logger.LogInformation($"Finished GetAll method request with data: {JsonSerializer
				//	.Serialize(regionsDomain)}");

				/*// Map domain models to DTOs
				//var regionsDto = new List<RegionDto>();

				//foreach (var region in regions)
				//{
				//	regionsDto.Add(new RegionDto
				//	{
				//		Id = region.Id,
				//		Name = region.Name,
				//		Code = region.Code,
				//		RegionImageUrl = region.RegionImageUrl,
				//	});
				//}

				//var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

				//if (regionsDto is not null && regionsDto.Any())
				//{
				//	return Ok(regionsDto);
				//}
				//var message = "Empty :(";*/
				return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
			}
			catch (Exception ex)
			{
				logger.LogError(ex, ex.Message);
				throw;
			}
			
		}

		[HttpGet]
		[Route("{id:Guid}")]
		//[Authorize(Roles = "Reader")]
		public async Task<IActionResult> GetById([FromRoute]Guid id)
		{
			//get region domain model from Db
			var regionDomain = await regionRepository.GetByIdAsync(id);

			if (regionDomain != null)
			{
				return Ok(mapper.Map<RegionDto>(regionDomain));
			}

			return NotFound();
		}

		[HttpPost]
		[ValidateModel]
		//[Authorize(Roles = "Writer")]
		public async Task<IActionResult> Create(
			[FromBody]AddRegionRequestDto addRegionRequestDto)
		{
			var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

			await regionRepository.CreateAsync(regionDomainModel);

			//map domain model back to dto
			var regionDto = mapper.Map<RegionDto>(regionDomainModel);

			return CreatedAtAction(nameof(GetById),
				new { id = regionDto.Id }, regionDto);

		}


		[HttpPut]
		[Route("{id:Guid}")]
		[ValidateModel]
		//[Authorize(Roles = "Writer")]
		public async Task<IActionResult> Update([FromRoute] Guid id,
			[FromBody] UpdateRegionRequestDto updateRegionRequestDto)
		{
			var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
			regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

			if (regionDomainModel == null)
			{
				return NotFound();
			}

			var regionDto = mapper.Map<RegionDto>(regionDomainModel);

			return Ok(regionDto);

		}


		[HttpDelete]
		[Route("{id:Guid}")]
		//[Authorize(Roles = "Writer, Reader")]
		public async  Task<IActionResult> Delete([FromRoute] Guid id)
		{
			var regionDomainModel = await regionRepository.DeleteAsync(id);

			if (regionDomainModel == null)
			{
				return NotFound();
			}

			var regionDto = mapper.Map<RegionDto>(regionDomainModel);
			
			return Ok(regionDto);
		}
	}
}
