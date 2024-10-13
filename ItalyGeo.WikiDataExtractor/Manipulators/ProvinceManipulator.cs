using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WikiDataExtractor.Helpers;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Province;
using WikiDataExtractor.Services;

namespace WikiDataExtractor.Manipulators
{
    public class ProvinceManipulator
    {
        private readonly IWikipediaApi _wikipediaApiService;
        private readonly IItalyGeoApi _italyGeoApi;

        public ProvinceManipulator(IWikipediaApi wikipediaApi, IItalyGeoApi italyGeoApi)
        {
            _wikipediaApiService = wikipediaApi;
            _italyGeoApi = italyGeoApi;
        }

        public async Task<List<AddProvinceRequest>> ParseHtmlAsync(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Xpath to the table that contains every Province in Italy via its id
            string xpath = $"//*[@id=\"mwRA\"]";

            var provincesTable = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            // Table Rows, skip header
            var trNodes = provincesTable.SelectNodes(".//tr").Skip(1);

            var provincesToAdd = new List<AddProvinceRequest>();

            // For each table row in the HTML table that contains every Province in Italy
            foreach (var tr in trNodes)
            {
                // Data cells in the current table row
                var tdNodes = tr.SelectNodes(".//td");

                // End of table
                if (tdNodes == null) break;

                // Node that contains PageUrl and Name of current Province
                var node0a = tdNodes.ElementAt(0).SelectSingleNode("a");

                string provinceWikiPagePath = StringHelper.SanitizeString(node0a.Attributes["href"].Value, false, true);
                string provinceName = StringHelper.SanitizeString(node0a.InnerText, false, true);

                // Node that contains PageUrl of the Region that the current Province belongs to
                var node2a = tdNodes.ElementAt(2).SelectSingleNode("a");

                string regionWikiPagePath = StringHelper.SanitizeString(node2a.Attributes["href"].Value, false, true);

                // Check if region exists
                var regionResponse = await _italyGeoApi.GetRegionByWikiPagePathAsync(regionWikiPagePath);
                if (regionResponse == null) continue;

                // Check if Province already exists
                var provinceResponse = await _italyGeoApi.GetProvinceByWikiPagePathAsync(provinceWikiPagePath);
                if (provinceResponse != null) continue;

                // Node that contains Province`s Acronym
                var node1 = tdNodes.ElementAt(1);
                var acronym = StringHelper.SanitizeString(node1.InnerText, false, true);

                // Node that contains Province`s population
                var node3 = tdNodes.ElementAt(3);
                int population = StringHelper.ConvertToInt(node3.InnerText);

                // Node that contains Provinces's area in km2
                var node4 = tdNodes.ElementAt(4);
                float areaKm2 = StringHelper.ConvertToFloat(node4.InnerText);

                // Node that contains Provinces's density (inhabitants/km2)
                var node5 = tdNodes.ElementAt(5);
                int inhabKm2 = StringHelper.ConvertToInt(node5.InnerText);

                // Node that contains Provinces's Comune count
                var node6 = tdNodes.ElementAt(6);
                int comuneCount = StringHelper.ConvertToInt(node6.InnerText);

                // Node that contains Provinces's year of creation
                var node9 = tdNodes.ElementAt(9);
                DateTime yearCreated = new DateTime(StringHelper.ConvertToInt(node9.InnerText), 1, 1);

                // Get province summary
                var provinceSummary = await _wikipediaApiService.GetPageSummaryAsync(provinceWikiPagePath) ?? new();

                provincesToAdd.Add(new AddProvinceRequest
                {
                    Name = provinceName,
                    RegionId = regionResponse.Id,
                    Latitude = provinceSummary.Coordinate.Latitude,
                    Longitude = provinceSummary.Coordinate.Longitude,
                    WikipediaPagePath = provinceWikiPagePath,
                    Acronym = acronym,
                    Areakm2 = areaKm2,
                    ComuneCount = comuneCount,
                    InhabitantsPerKm2 = inhabKm2,
                    Population = population,
                    yearCreated = yearCreated
                });
            }
            return provincesToAdd;
        }
    }
}
