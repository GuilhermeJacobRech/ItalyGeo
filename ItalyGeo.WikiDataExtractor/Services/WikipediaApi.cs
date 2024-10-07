using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Region;
using WikiDataExtractor.Models.WikipediaApi;

namespace WikiDataExtractor.Services
{
    public class WikipediaApi : IWikipediaApi
    {
        private readonly HttpClient _httpClient;

        public WikipediaApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> GetPageHtmlAsync(string pagePath)
        {
            var response = await _httpClient.GetAsync($"page/html/{pagePath}");
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

            return null;
        }

        public async Task<PageSummaryResponse?> GetPageSummaryAsync(string pagePath)
        {
            var response = await _httpClient.GetAsync($"page/summary/{pagePath}");
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<PageSummaryResponse>();
            return null;
        }


    }
}
