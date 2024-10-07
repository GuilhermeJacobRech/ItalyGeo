using ItalyGeo.API.Models.Domain;

namespace ItalyGeo.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync(string? filterOn = null, string? filterQuery = null);
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region?> GetByWikiPagePathAsync(string WikiPagePath);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
