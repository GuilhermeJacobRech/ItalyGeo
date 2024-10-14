using HtmlAgilityPack;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Region;
using WikiDataExtractor.Helpers;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Comune;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Region;
using WikiDataExtractor.Services;
using Serilog;
namespace WikiDataExtractor.Manipulators
{
    public class RegionManipulator
    {
        private readonly IWikipediaApi _wikipediaApi;
        private readonly IItalyGeoApi _italyGeoApi;
        private readonly ILogger _logger;

        public RegionManipulator(IWikipediaApi wikipediaApi, IItalyGeoApi italyGeoApi, ILogger logger)
        {
            _wikipediaApi = wikipediaApi;
            this._italyGeoApi = italyGeoApi;
            this._logger = logger;
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

                // Node that contains Region Wikipedia Page URL and Name
                var node0ba = tdNodes.ElementAt(0).SelectSingleNode("b/a");
                string wikiPagePath = StringHelper.SanitizeString(node0ba.Attributes["href"].Value, false, true);
                string name = StringHelper.SanitizeString(node0ba.Attributes["title"].Value, false, true);

                // Check if region already exists
                var regionResponse = await _italyGeoApi.GetRegionByWikiPagePathAsync(wikiPagePath);
                RegionDto? regionDto = await ParseRegionWikiPage(wikiPagePath);
                if (regionDto == null) continue;

                if (regionResponse != null && regionDto != null)
                {
                    regionsToProcess.Add(new UpdateRegionRequest
                    {
                        Id = regionResponse.Id,
                        Name = name,
                        WikipediaPagePath = wikiPagePath,
                        AltitudeAboveSeaMeterMSL = regionDto.AltitudeAboveSeaMeterMSL,
                        Areakm2 = regionDto.AreaKm2,
                        ComuneCount = regionDto.ComuneCount,
                        ProvinceCount = regionDto.ProvinceCount,
                        GDPNominalMlnEuro = regionDto.GDPNominalMlnEuro,
                        GDPPerCapitaEuro = regionDto.GDPPerCapitaEuro,
                        InhabitantName = regionDto.InhabitantName,
                        InhabitantsPerKm2 = regionDto.InhabitantsPerKm2,
                        Latitude = regionDto.Latitude,
                        Longitude = regionDto.Longitude,
                        PatronSaint = regionDto.PatronSaint,
                        Population = regionDto.Population,
                        Timezone = regionDto.Timezone
                    });
                }
                if (regionResponse == null && regionDto != null)
                {
                    regionsToProcess.Add(new AddRegionRequest
                    {
                        Name = name,
                        WikipediaPagePath = wikiPagePath,
                        AltitudeAboveSeaMeterMSL = regionDto.AltitudeAboveSeaMeterMSL,
                        Areakm2 = regionDto.AreaKm2,
                        ComuneCount = regionDto.ComuneCount,
                        ProvinceCount = regionDto.ProvinceCount,
                        GDPNominalMlnEuro = regionDto.GDPNominalMlnEuro,
                        GDPPerCapitaEuro = regionDto.GDPPerCapitaEuro,
                        InhabitantName = regionDto.InhabitantName,
                        InhabitantsPerKm2 = regionDto.InhabitantsPerKm2,
                        Latitude = regionDto.Latitude,
                        Longitude = regionDto.Longitude,
                        PatronSaint = regionDto.PatronSaint,
                        Population = regionDto.Population,
                        Timezone = regionDto.Timezone
                    });
                }
            }
            return regionsToProcess;
        }

        private async Task<RegionDto?> ParseRegionWikiPage(string regionWikiPagePath)
        {
            var regionWikiPageHTML = await _wikipediaApi.GetPageHtmlAsync(regionWikiPagePath);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(regionWikiPageHTML);

            string xpath = "html/body/section[1]/table/tbody/tr";
            var infoboxTableRows = htmlDoc.DocumentNode.SelectNodes(xpath);
            if (infoboxTableRows == null)
            {
                _logger.Error($"Could not find infobox - Region wiki page: { regionWikiPagePath }");
                return null;
            }

            var regionToProcess = new RegionDto();

            Parallel.ForEach(infoboxTableRows, new ParallelOptions() { MaxDegreeOfParallelism = 20 }, (tr, ct) =>
            {
                try
                {
                    // Check if current table row has a table header
                    var th = tr.SelectSingleNode("th");
                    if (th == null) return;

                    // Check if the innerText of 'th' or its child 'a' contains words respective to each property in addComuneRequest
                    string headerText = StringHelper.SanitizeString(th.InnerText, false, true).ToLower();
                    var tha = th.SelectSingleNode("a");
                    if (tha != null)
                    {
                        headerText += StringHelper.SanitizeString(tha.InnerText, false, true).ToLower();
                    }

                    if (headerText.Contains("altitudine"))
                    {
                        string s = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, false);
                        regionToProcess.AltitudeAboveSeaMeterMSL = StringHelper.ConvertToFloat(s);
                        return;
                    }

                    if (headerText.Contains("superficie"))
                    {
                        string s = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, false);
                        regionToProcess.AreaKm2 = StringHelper.ConvertToFloat(s);
                        return;
                    }

                    if (headerText.Contains("densità"))
                    {
                        string s = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, false);
                        regionToProcess.InhabitantsPerKm2 = StringHelper.ConvertToFloat(s);
                        return;
                    }

                    if (headerText.Equals("abitanti") || headerText.Equals("abitantiabitanti"))
                    {
                        string s = StringHelper.SanitizeString(tr.SelectSingleNode("td").FirstChild.InnerText, true, false);
                        regionToProcess.Population = StringHelper.ConvertToInt(s);
                        return;
                    }

                    if (headerText.Contains("fuso orario"))
                    {
                        regionToProcess.Timezone = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, true);
                        return;
                    }

                    if (headerText.Contains("patrono"))
                    {
                        regionToProcess.PatronSaint = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, false, true);
                        return;
                    }

                    if (headerText.Contains("comuni"))
                    {
                        string s = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, false);
                        regionToProcess.ComuneCount = StringHelper.ConvertToInt(s);
                        return;
                    }

                    if (headerText.Contains("province"))
                    {
                        int provinceCount = tr.SelectSingleNode("td").InnerText.Count(c => c == ',') + 1;
                        regionToProcess.ProvinceCount = provinceCount;
                        return;
                    }

                    if (headerText.Equals("nome abitanti") || headerText.Equals("nome abitantinome abitanti"))
                    {
                        regionToProcess.InhabitantName = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, false, true);
                        return;
                    }

                    if (headerText.Contains("coordinate"))
                    {
                        float dataLat = StringHelper.ConvertToFloat(tr.SelectSingleNode("td")?.SelectSingleNode("a")?.Attributes["data-lat"].Value ?? "");
                        float dataLon = StringHelper.ConvertToFloat(tr.SelectSingleNode("td")?.SelectSingleNode("a")?.Attributes["data-lon"].Value ?? "");

                        regionToProcess.Latitude = (decimal)dataLat;
                        regionToProcess.Longitude = (decimal)dataLon;

                        return;
                    }

                    if (headerText.Equals("pil") || headerText.Equals("pilpil"))
                    {
                        string innerText = tr.SelectSingleNode("td").InnerText;
                        int index = innerText.IndexOf('€');
                        innerText = innerText[0..index];
                        string s = StringHelper.SanitizeString(innerText, true, false);
                        regionToProcess.GDPNominalMlnEuro = StringHelper.ConvertToFloat(s);
                        return;
                    }

                    if (headerText.Contains("pil procapite"))
                    {
                        string innerText = tr.SelectSingleNode("td").InnerText;
                        int index = innerText.IndexOf('€');
                        innerText = innerText[0..index];
                        string s = StringHelper.SanitizeString(innerText, true, false);
                        regionToProcess.GDPPerCapitaEuro = StringHelper.ConvertToFloat(s);
                        return;
                    }


                }
                catch (NullReferenceException e)
                {
                    _logger.Error($"Error: {e.Message} - Comune wiki page: {regionWikiPagePath}");
                }
            });

            return regionToProcess;
        }
    }
}
