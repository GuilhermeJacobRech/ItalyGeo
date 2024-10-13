using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Auth;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Comune;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Province;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Region;

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
        Task<HttpResponseMessage> CreateComuneAsync(AddComuneRequest comune);
    }
}
