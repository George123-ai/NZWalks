using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
	public class NZWalksDbContext : DbContext
	{
		public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions): base(dbContextOptions)
		{

		}

		public DbSet<Difficulty> Difficulties { get; set; }
		public DbSet<Region> Regions { get; set; }
		public DbSet<Walk> Walks { get; set; }
		public DbSet<Image> Images { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//seed data for Difficulties
			//easy, medium, hard

			var difficulties = new List<Difficulty>()
			{
				new Difficulty
				{
					Id = Guid.Parse("a7a5b066-9626-4442-8863-4f3f6c3f7fc6"),
					Name = "Easy"
				},
				new Difficulty
				{
					Id = Guid.Parse("5fcd1311-d995-4ab0-b4f4-ad23b9ee623a"),
					Name = "Medium"
				},
				new Difficulty
				{
					Id = Guid.Parse("6f4bad8e-c052-458d-b84d-fd038b24eab8"),
					Name = "Hard"
				}
			};

			//seed difficulties to the db
			modelBuilder.Entity<Difficulty>().HasData(difficulties);


			//seed data for Regions
			var regions = new List<Region>
			{
				new Region
				{
					Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
					Name = "Auckland",
					Code = "AKL",
					RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
				},
				new Region
				{
					Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
					Name = "Northland",
					Code = "NTL",
					RegionImageUrl = null
				},
				new Region
				{
					Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
					Name = "Bay Of Plenty",
					Code = "BOP",
					RegionImageUrl = null
				},
				new Region
				{
					Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
					Name = "Wellington",
					Code = "WGN",
					RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
				},
				new Region
				{
					Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
					Name = "Nelson",
					Code = "NSN",
					RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
				},
				new Region
				{
					Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
					Name = "Southland",
					Code = "STL",
					RegionImageUrl = null
				},
			};

			modelBuilder.Entity<Region>().HasData(regions);
		}

	}	
}
