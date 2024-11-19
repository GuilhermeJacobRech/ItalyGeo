using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Comune;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Province;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Region;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WikiDataExtractor.Models.ItalyGeo.Auth;
using WikiDataExtractor.Models.ItalyGeo.Comune;
using WikiDataExtractor.Models.ItalyGeo.Province;
using WikiDataExtractor.Models.ItalyGeo.Region;

namespace WikiDataExtractor.Services
{
    public class ItalyGeoApi : IItalyGeoApi
    {
        private readonly HttpClient _httpClient;

        public ItalyGeoApi(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<bool> AuthenticateAsync(Credential credential)
        {
            var response = await _httpClient.PostAsJsonAsync("auth", credential);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<JwtToken>(body);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AcessToken);
                return true;
            }
            return false;
        }

        public async Task<RegionResponse?> GetRegionByWikiPagePathAsync(string wikiPagePath)
        {
            var response = await _httpClient.GetAsync($"regions/wikipath/{wikiPagePath}");
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<RegionResponse>();
            return null;
        }

        public async Task<ProvinceResponse?> GetProvinceByWikiPagePathAsync(string wikiPagePath)
        {
            var response = await _httpClient.GetAsync($"provinces/wikipath/{wikiPagePath}");
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<ProvinceResponse>();
            return null;
        }

        public async Task<ComuneResponse?> GetComuneByWikiPagePathAsync(string wikiPagePath)
        {
            var response = await _httpClient.GetAsync($"comunes/wikipath/{wikiPagePath}");
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<ComuneResponse>();
            return null;
        }

        public async Task<HttpResponseMessage> CreateRegionAsync(AddRegionRequest region)
        {
            return await _httpClient.PostAsJsonAsync("regions", region);
        }

        public async Task<HttpResponseMessage> UpdateRegionAsync(Guid id, UpdateRegionRequest region)
        {
            return await _httpClient.PutAsJsonAsync($"regions/{id}", region);
        }

        public async Task<HttpResponseMessage> CreateProvinceAsync(AddProvinceRequest province)
        {
            var response = await _httpClient.PostAsJsonAsync("provinces", province);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateProvinceAsync(Guid id, UpdateProvinceRequest province)
        {
            return await _httpClient.PutAsJsonAsync($"provinces/{id}", province);
        }

        public async Task<HttpResponseMessage> CreateComuneAsync(AddComuneRequest comune)
        {
            var response = await _httpClient.PostAsJsonAsync("comunes", comune);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateComuneAsync(Guid id, UpdateComuneRequest comune)
        {
            return await _httpClient.PutAsJsonAsync($"comunes/{id}", comune);
        }
    }
}
