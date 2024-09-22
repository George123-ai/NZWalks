using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
	public class RegionRepository : IRegionRepository
	{
		private readonly NZWalksDbContext dbContext;

		public RegionRepository(NZWalksDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<Region> CreateAsync(Region region)
		{
			await dbContext.Regions.AddAsync(region);
			await dbContext.SaveChangesAsync();
			return region;
		}

		public async Task<List<Region>> GetAllAsync()
		{
			return await dbContext.Regions.ToListAsync();
		}

		public async Task<Region?> GetByIdAsync([FromRoute] Guid id)
		{
			return await dbContext.Regions
				.FirstOrDefaultAsync(u => u.Id == id);
		}

		public async Task<Region?> UpdateAsync(Guid id, Region region)
		{
			var existingRegion = await dbContext.Regions.FindAsync(id);

			if (existingRegion != null)
			{
				existingRegion.Name = region.Name;
				existingRegion.Code = region.Code;
				existingRegion.RegionImageUrl = region.RegionImageUrl;

				await dbContext.SaveChangesAsync();

				return existingRegion;
			}

			return null;
		}

		public async Task<Region?> DeleteAsync(Guid id)
		{
			var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);

			if (existingRegion == null)
			{
				return null;
			}

			dbContext.Regions.Remove(existingRegion);
			await dbContext.SaveChangesAsync();

			return existingRegion;
		}
	}
}
