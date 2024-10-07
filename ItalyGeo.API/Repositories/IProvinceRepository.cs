using ItalyGeo.API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace ItalyGeo.API.Repositories
{
    public interface IProvinceRepository
    {
        Task<List<Province>> GetAllAsync(string? filterOn = null, string? filterQuery = null);
        Task<Province?> GetByIdAsync(Guid id);
        Task<Province?> GetByWikiPagePathAsync(string WikiPagePath);
        Task<Province> CreateAsync(Province province);
        Task<Province?> UpdateAsync(Guid id, Province province);
        Task<Province?> DeleteAsync(Guid id);
    }
}
