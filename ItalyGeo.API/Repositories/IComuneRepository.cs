using ItalyGeo.API.Models.Domain;
using ItalyGeo.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ItalyGeo.API.Repositories
{
    public interface IComuneRepository
    {
        Task<List<Comune>> GetAllAsync(string? filterOn = null, string? filterQuery = null, int pageNumber = 1, int pageSize = 1000, bool orderByDescending = true);
        Task<Comune?> GetByIdAsync(Guid id);
        Task<Comune?> GetByWikiPagePathAsync(string WikiPagePath);
        Task<Comune> CreateAsync(Comune comune);
        Task<Comune?> UpdateAsync(Guid id, Comune comune);
        Task<Comune?> DeleteAsync(Guid id);
    }
}
