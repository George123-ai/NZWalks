using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WalksController : ControllerBase
	{
		private readonly IMapper mapper;
		private readonly IWalkRepository walkRepository;
		public WalksController(IMapper mapper, IWalkRepository walkRepository)
		{
			this.mapper = mapper;
			this.walkRepository = walkRepository;
		}

		[HttpPost]
		[ValidateModel]
		public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
		{
			//Map dto to doamin model
			var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

			await walkRepository.CreateAsync(walkDomainModel);

			//Map domain model back to dto and return it
			return Ok(mapper.Map<WalkDto>(walkDomainModel));

		}

		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery]string? filterOn,
			[FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
			[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
		{
			var walksDomain = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy,
						isAscending ?? true, pageNumber, pageSize);

			//Create an exception
			throw new Exception("This is new exception");

			return Ok(mapper.Map<List<WalkDto>>(walksDomain));
		}


		[HttpGet]
		[Route("{id:Guid}")]
		public async Task<IActionResult> GetById([FromRoute]Guid id)
		{
			var walkDomain = await walkRepository.GetByIdAsync(id);

			if (walkDomain == null)
			{
				return NotFound();
			}

			var walkDto = mapper.Map<WalkDto>(walkDomain);
			return Ok(walkDto);
		}

		[HttpPut]
		[Route("{id:Guid}")]
		[ValidateModel]
		public async Task<IActionResult> Update([FromBody]UpdateWalkRequestDto updateWalkRequestDto,
			[FromRoute]Guid id)
		{
			var walkDomain = mapper.Map<Walk>(updateWalkRequestDto);
			walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

			if (walkDomain == null)
			{
				return NotFound();
			}

			return Ok(mapper.Map<WalkDto>(walkDomain));

		}

		[HttpDelete]
		[Route("{id:Guid}")]
		public async Task<IActionResult> Delete([FromRoute]Guid id)
		{
			var deletedWalk = await walkRepository.DeleteAsync(id);
			if (deletedWalk == null)
			{
				return NotFound();
			}

			return Ok(mapper.Map<WalkDto>(deletedWalk));
		}
	}
}
