using ItalyGeo.API.Data;
using ItalyGeo.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ItalyGeo.API.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly ItalyGeoDbContext _dbContext;

        public SqlRegionRepository(ItalyGeoDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync(string? filterOn = null, string? filterQuery = null, bool orderByDescending = false)
        {
            var regions = _dbContext.Regions.AsQueryable();

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("regionname", StringComparison.OrdinalIgnoreCase))
                {
                    regions = regions.Where(x => x.Name.Contains(filterQuery));
                }
            }

            // Sorting
            if (orderByDescending == true)
            {
                regions = regions.OrderByDescending(x => x.Name);
            }
            else
            {
                regions = regions.OrderBy(x => x.Name);
            }

            return await regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> GetByWikiPagePathAsync (string WikiPagePath)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(x => x.WikipediaPagePath == WikiPagePath);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null) return null;

            existingRegion.Name = region.Name;
            existingRegion.Latitude = region.Latitude;
            existingRegion.Longitude = region.Longitude;
            existingRegion.WikipediaPagePath = region.WikipediaPagePath;
            existingRegion.AreaKm2 = region.AreaKm2;
            existingRegion.ComuneCount = region.ComuneCount;
            existingRegion.ProvinceCount = region.ProvinceCount;
            existingRegion.GDPNominalMlnEuro = region.GDPNominalMlnEuro;
            existingRegion.GDPPerCapitaEuro = region.GDPPerCapitaEuro;
            existingRegion.InhabitantsPerKm2 = region.InhabitantsPerKm2;
            existingRegion.Population = region.Population;
            existingRegion.Timezone = region.Timezone;
            existingRegion.Acronym = region.Acronym;
            existingRegion.CapaluogoComuneId = region.CapaluogoComuneId;

            await _dbContext.SaveChangesAsync();

            return existingRegion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null) return null;
            _dbContext.Regions.Remove(existingRegion);
            await _dbContext.SaveChangesAsync();

            return existingRegion;
        }

    }
}
