using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Comune;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Province;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiDataExtractor.Models.ItalyGeo.Auth;
using WikiDataExtractor.Models.ItalyGeo.Comune;
using WikiDataExtractor.Models.ItalyGeo.Province;
using WikiDataExtractor.Models.ItalyGeo.Region;

namespace WikiDataExtractor.Services
{
    public interface IItalyGeoApi
    {
        Task<bool> AuthenticateAsync(Credential credential);
        Task<RegionResponse?> GetRegionByWikiPagePathAsync (string wikiPagePath);
        Task<ProvinceResponse?> GetProvinceByWikiPagePathAsync(string wikiPagePath);
        Task<ComuneResponse?> GetComuneByWikiPagePathAsync(string wikiPagePath);
        Task<HttpResponseMessage> CreateRegionAsync(AddRegionRequest region);
        Task<HttpResponseMessage> UpdateRegionAsync(Guid id, UpdateRegionRequest region);
        Task<HttpResponseMessage> CreateProvinceAsync(AddProvinceRequest province);
        Task<HttpResponseMessage> UpdateProvinceAsync(Guid id, UpdateProvinceRequest province);
        Task<HttpResponseMessage> CreateComuneAsync(AddComuneRequest comune);
        Task<HttpResponseMessage> UpdateComuneAsync(Guid id, UpdateComuneRequest comune);
    }
}
