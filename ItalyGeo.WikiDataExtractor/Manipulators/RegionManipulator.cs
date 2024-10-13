using HtmlAgilityPack;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Region;
using WikiDataExtractor.Helpers;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Region;
using WikiDataExtractor.Services;

namespace WikiDataExtractor.Manipulators
{
    public class RegionManipulator
    {
        private readonly IWikipediaApi _wikipediaApi;
        private readonly IItalyGeoApi _italyGeoApi;

        public RegionManipulator(IWikipediaApi wikipediaApi, IItalyGeoApi italyGeoApi)
        {
            _wikipediaApi = wikipediaApi;
            this._italyGeoApi = italyGeoApi;
        }

        public async Task<List<IRegionRequest>> ParseHtmlAsync(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Xpath to the table that contains every Region in Italy via its id
            string xpath = $"//*[@id=\"mwZg\"]";

            var table = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            // Skip header
            var tableRows = table.SelectNodes(".//tr").Skip(1);

            var regionsToProcess = new List<IRegionRequest>();

            // For each region
            foreach (var row in tableRows)
            {
                // Data cells in the current table row
                var tdNodes = row.SelectNodes(".//td");

                // End of table
                if (tdNodes == null) break;

                // Node that contains Region PageUrl and Name
                var node0ba = tdNodes.ElementAt(0).SelectSingleNode("b/a");
                string wikiPagePath = StringHelper.SanitizeString(node0ba.Attributes["href"].Value, false, true);
                string name = StringHelper.SanitizeString(node0ba.Attributes["title"].Value, false, true);

                // Node that contains Wikipedia page path of Capaluogo of this Region
                var node1a = tdNodes.ElementAt(1).SelectSingleNode("a");
                string capaluogoWikiPagePath = StringHelper.SanitizeString(node1a.Attributes["href"].Value, false, true);

                // Node that contains Region's Population
                var node2 = tdNodes.ElementAt(2);
                int population = StringHelper.ConvertToInt(node2.InnerText);

                // Node that contains Region's area in km2
                var node3 = tdNodes.ElementAt(3);
                float areaKm2 = StringHelper.ConvertToFloat(node3.InnerText);

                // Node that contains Region's density (inhabitants/km2)
                var node4 = tdNodes.ElementAt(4);
                int inhabKm2 = StringHelper.ConvertToInt(node4.InnerText);

                // Node that contains Region's Provinces
                var node5 = tdNodes.ElementAt(5);
                string provinces = node5.InnerText;

                int provinceCount = provinces.Count(c => c == ',') + 1;

                // Node that contains Region's Comune count
                var node6 = tdNodes.ElementAt(6);
                int comuneCount = StringHelper.ConvertToInt(node6.InnerText);

                // Get Region summary
                var regionSummary = await _wikipediaApi.GetPageSummaryAsync(wikiPagePath) ?? new();

                // Check if region already exists
                var regionResponse = await _italyGeoApi.GetRegionByWikiPagePathAsync(wikiPagePath);
                if (regionResponse != null)
                {
                    regionsToProcess.Add(new UpdateRegionRequest
                    {
                        Id = regionResponse.Id,
                        Name = name,
                        WikipediaPagePath = wikiPagePath,
                        Latitude = regionSummary.Coordinate.Latitude,
                        Longitude = regionSummary.Coordinate.Longitude,
                        Population = population,
                        Areakm2 = areaKm2,
                        InhabitantsPerKm2 = inhabKm2,
                        ProvinceCount = provinceCount,
                        ComuneCount = comuneCount
                    });
                }
                else
                {
                    regionsToProcess.Add(new AddRegionRequest
                    {
                        Name = name,
                        WikipediaPagePath = wikiPagePath,
                        Latitude = regionSummary.Coordinate.Latitude,
                        Longitude = regionSummary.Coordinate.Longitude,
                        Population = population,
                        Areakm2 = areaKm2,
                        InhabitantsPerKm2 = inhabKm2,
                        ProvinceCount = provinceCount,
                        ComuneCount = comuneCount
                    });
                }
            }
            return regionsToProcess;
        }
    }
}
