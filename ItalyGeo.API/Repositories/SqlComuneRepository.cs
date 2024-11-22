using ItalyGeo.API.Data;
using ItalyGeo.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ItalyGeo.API.Repositories
{
    public class SqlComuneRepository : IComuneRepository
    {
        private readonly ItalyGeoDbContext _dbContext;
        public SqlComuneRepository(ItalyGeoDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Comune>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            int pageNumber = 1, int pageSize = 1000, bool orderByDescending = false)
        {
            var comunes = _dbContext.Comunes.Include(x => x.Province).Include(x => x.Province.Region).AsQueryable();

            // Filtering by comune name, province name or region name
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("comunename", StringComparison.OrdinalIgnoreCase))
                {
                    comunes = comunes.Where(x => x.Name.Contains(filterQuery));
                }

                if (filterOn.Equals("provincename", StringComparison.OrdinalIgnoreCase))
                {
                    comunes = comunes.Where(x => x.Province.Name.Contains(filterQuery));
                }

                if (filterOn.Equals("regionname", StringComparison.OrdinalIgnoreCase))
                {
                    comunes = comunes.Where(x => x.Province.Region.Name.Contains(filterQuery));
                }
            }

            // Sorting
            if (orderByDescending == true)
            {
                comunes = comunes.OrderByDescending(x => x.Name);
            }
            else
            {
                comunes = comunes.OrderBy(x => x.Name);
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await comunes.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Comune?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Comunes.Include("Province").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Comune?> GetByWikiPagePathAsync(string WikiPagePath)
        {
            return await _dbContext.Comunes.FirstOrDefaultAsync(x => x.WikipediaPagePath == WikiPagePath);
        }

        public async Task<Comune> CreateAsync(Comune comune)
        {
            await _dbContext.Comunes.AddAsync(comune);
            await _dbContext.SaveChangesAsync();

            // Load Province and Region (taken from https://stackoverflow.com/questions/26610337/get-entity-navigation-properties-after-insert) 
            await _dbContext.Entry(comune).Reference(x => x.Province).LoadAsync();
            await _dbContext.Entry(comune.Province).Reference(x => x.Region).LoadAsync();

            return comune;
        }

        public async Task<Comune?> UpdateAsync(Guid id, Comune comune)
        {
            var existingComune = await _dbContext.Comunes.FirstOrDefaultAsync(x => x.Id == id);
            if (existingComune == null) return null;

            existingComune.Name = comune.Name;
            existingComune.ProvinceId = comune.ProvinceId;
            existingComune.Latitude = comune.Latitude;
            existingComune.Longitude = comune.Longitude;
            existingComune.WikipediaPagePath = comune.WikipediaPagePath;
            existingComune.AltitudeAboveSeaMeterMSL = comune.AltitudeAboveSeaMeterMSL;
            existingComune.AreaKm2 = comune.AreaKm2;
            existingComune.Population = comune.Population;
            existingComune.ZipCode = comune.ZipCode;
            existingComune.Timezone = comune.Timezone;
            existingComune.InhabitantsPerKm2 = comune.InhabitantsPerKm2;

            await _dbContext.SaveChangesAsync();

            // Load Province and Region (taken from https://stackoverflow.com/questions/26610337/get-entity-navigation-properties-after-insert) 
            await _dbContext.Entry(comune).Reference(x => x.Province).LoadAsync();
            await _dbContext.Entry(comune.Province).Reference(x => x.Region).LoadAsync();

            return existingComune;
        }

        public async Task<Comune?> DeleteAsync(Guid id)
        {
            var existingComune = await _dbContext.Comunes.FirstOrDefaultAsync(x => x.Id == id);
            if (existingComune == null) return null;
            _dbContext.Comunes.Remove(existingComune);
            await _dbContext.SaveChangesAsync();

            // Load Province and Region (taken from https://stackoverflow.com/questions/26610337/get-entity-navigation-properties-after-insert) 
            await _dbContext.Entry(existingComune).Reference(x => x.Province).LoadAsync();
            await _dbContext.Entry(existingComune.Province).Reference(x => x.Region).LoadAsync();

            return existingComune;
        }
    }
}
