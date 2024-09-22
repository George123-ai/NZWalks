using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
	public class WalkRepository : IWalkRepository
	{
		private readonly NZWalksDbContext dbContext;

		public WalkRepository(NZWalksDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<Walk> CreateAsync(Walk walk)
		{
			await dbContext.Walks.AddAsync(walk);
			await dbContext.SaveChangesAsync();
			return walk;
		}

		public async Task<Walk?> DeleteAsync(Guid id)
		{
			var walk = await dbContext.Walks.FindAsync(id);
			if (walk == null)
			{
				return null;
			}
			dbContext.Walks.Remove(walk);
			await dbContext.SaveChangesAsync();
			return walk;
		}

		public async Task<List<Walk>> GetAllAsync(string? filterOn = null,
			string? filterQuery = null, string? sortBy = null, bool isAscending = false,
			int pageNumber = 1, int pageSize = 1000)
		{
			var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

			// filtering
			if (string.IsNullOrWhiteSpace(filterOn) == false &&
				string.IsNullOrWhiteSpace(filterQuery) == false)
			{
				if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					walks = walks.Where(x => x.Name.Contains(filterQuery));
				}
			}

			//sorting
			if (string.IsNullOrWhiteSpace(sortBy) == false)
			{
				if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					walks = isAscending? walks.OrderBy(x => x.Name) : walks.
						OrderByDescending(x => x.Name);
				}
				else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
				{
					walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.
						OrderByDescending(x => x.LengthInKm);
				}
			}

			//pagination
			var skipResult = (pageNumber - 1) * pageSize;

			return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
			//return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
		}

		public async Task<Walk?> GetByIdAsync(Guid id)
		{
			return await dbContext.Walks.Include("Difficulty").Include("Region").
				FirstOrDefaultAsync(u => u.Id == id);
		}

		public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
		{
			var walkDomain = await dbContext.Walks.FindAsync(id);

			if (walkDomain == null)
			{
				return null;
			}

			walkDomain.Name = walk.Name;
			walkDomain.Description = walk.Description;
			walkDomain.LengthInKm = walk.LengthInKm;
			walkDomain.WalkImageUrl = walk.WalkImageUrl;
			walkDomain.DifficultyId = walk.DifficultyId;
			walkDomain.RegionId = walk.RegionId;

			await dbContext.SaveChangesAsync();

			return walkDomain;
			
		}
	}
}
