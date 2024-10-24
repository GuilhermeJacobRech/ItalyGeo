using HtmlAgilityPack;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Province;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Region;
using Serilog;
using System.Xml.Linq;
using WikiDataExtractor.Helpers;
using WikiDataExtractor.Models.ItalyGeo.Comune;
using WikiDataExtractor.Models.ItalyGeo.Province;
using WikiDataExtractor.Models.ItalyGeo.Region;
using WikiDataExtractor.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WikiDataExtractor.Manipulators
{
    public class ProvinceManipulator
    {
        private readonly IWikipediaApi _wikipediaApi;
        private readonly IItalyGeoApi _italyGeoApi;
        private readonly ILogger _logger;

        public ProvinceManipulator(IWikipediaApi wikipediaApi, IItalyGeoApi italyGeoApi, ILogger logger)
        {
            _wikipediaApi = wikipediaApi;
            _italyGeoApi = italyGeoApi;
            _logger = logger;
        }

        public async Task<List<IProvinceRequest>> ParseHtmlAsync(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Xpath to the table that contains every Province in Italy via its id
            string xpath = $"//*[@id=\"mwRA\"]";

            var provincesTable = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            // Table Rows, skip header
            var rows = provincesTable.SelectNodes(".//tr").Skip(1);

            var provincesToProcess = new List<IProvinceRequest>();

            // For each table row in the HTML table that contains every Province in Italy
            foreach (var row in rows)
            {
                // Data cells in the current table row
                var tdNodes = row.SelectNodes(".//td");

                // End of table
                if (tdNodes == null) break;

                // Node that contains Wikipedia Page URL and name of the Province
                var node0a = tdNodes.ElementAt(0).SelectSingleNode("a");
                string provinceWikiPagePath = StringHelper.SanitizeString(node0a.Attributes["href"].Value, false, true);
                string provinceName = StringHelper.SanitizeString(node0a.InnerText, false, true);

                // Node that contains Wikipedia Page URL of the Region that the current Province belongs to
                var node2a = tdNodes.ElementAt(2).SelectSingleNode("a");
                string regionWikiPagePath = StringHelper.SanitizeString(node2a.Attributes["href"].Value, false, true);

                // Check if region exists
                var regionResponse = await _italyGeoApi.GetRegionByWikiPagePathAsync(regionWikiPagePath);
                if (regionResponse == null)
                {
                    _logger.Error($"Could not found region {regionWikiPagePath} when trying to process province {provinceName}");
                    continue;
                }

                // Get province on Italy Geo
                var provinceResponse = await _italyGeoApi.GetProvinceByWikiPagePathAsync(provinceWikiPagePath);
                ProvinceDto? provinceDto = await ParseProvinceWikiPage(provinceWikiPagePath);

                if (provinceDto == null) continue;

                if (provinceResponse != null && provinceDto != null)
                {
                    provincesToProcess.Add(new UpdateProvinceRequest
                    {
                        Id = provinceResponse.Id,
                        RegionId = regionResponse.Id,
                        Name = provinceName,
                        WikipediaPagePath = provinceWikiPagePath,
                        AreaKm2 = provinceDto.AreaKm2,
                        ComuneCount = provinceDto.ComuneCount,
                        GDPNominalMlnEuro = provinceDto.GDPNominalMlnEuro,
                        GDPPerCapitaEuro = provinceDto.GDPPerCapitaEuro,
                        InhabitantsPerKm2 = provinceDto.InhabitantsPerKm2,
                        Latitude = provinceDto.Latitude,
                        Longitude = provinceDto.Longitude,
                        Population = provinceDto.Population,
                        Timezone = provinceDto.Timezone,
                        Acronym = provinceDto.Acronym,
                        YearCreated = provinceDto.yearCreated,
                        Zipcode = provinceDto.Zipcode
                    });
                }
                if (provinceResponse == null && provinceDto != null)
                {
                    provincesToProcess.Add(new AddProvinceRequest
                    {
                        RegionId = regionResponse.Id,
                        Name = provinceName,
                        WikipediaPagePath = provinceWikiPagePath,
                        AreaKm2 = provinceDto.AreaKm2,
                        ComuneCount = provinceDto.ComuneCount,
                        GDPNominalMlnEuro = provinceDto.GDPNominalMlnEuro,
                        GDPPerCapitaEuro = provinceDto.GDPPerCapitaEuro,
                        InhabitantsPerKm2 = provinceDto.InhabitantsPerKm2,
                        Latitude = provinceDto.Latitude,
                        Longitude = provinceDto.Longitude,
                        Population = provinceDto.Population,
                        Timezone = provinceDto.Timezone,
                        Acronym = provinceDto.Acronym,
                        YearCreated = provinceDto.yearCreated,
                        Zipcode = provinceDto.Zipcode
                    });
                }
            }
            return provincesToProcess;
        }

        private async Task<ProvinceDto?> ParseProvinceWikiPage(string provinceWikiPagePath)
        {
            var provinceWikiPageHTML = await _wikipediaApi.GetPageHtmlAsync(provinceWikiPagePath);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(provinceWikiPageHTML);

            string xpath = "html/body/section[1]/table/tbody/tr";
            var infoboxTableRows = htmlDoc.DocumentNode.SelectNodes(xpath);
            if (infoboxTableRows == null)
            {
                _logger.Error($"Could not find infobox - Province wiki page: {provinceWikiPagePath}");
                return null;
            }

            var provinceToProcess = new ProvinceDto();

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

                    string tdText = tr.SelectSingleNode("td")?.InnerText ?? "";

                    if (headerText.Contains("targa"))
                    {
                        provinceToProcess.Acronym = StringHelper.SanitizeString(tdText, false, true);
                        return;
                    }

                    if (headerText.Equals("cod postale") || headerText.Equals("cod postalecod postale"))
                    {
                        provinceToProcess.Zipcode = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, true);
                        return;
                    }

                    if (headerText.Contains("superficie"))
                    {
                        string s = StringHelper.SanitizeString(tdText, true, false);
                        provinceToProcess.AreaKm2 = StringHelper.ConvertToFloat(s);
                        return;
                    }

                    if (headerText.Equals("data di istituzione") || headerText.Equals("data di istituzionedata di istituzione"))
                    {
                        string s = StringHelper.SanitizeString(tdText, true, true);
                        int? year = StringHelper.ExtractYear(s);
                        provinceToProcess.yearCreated = new DateTime(year ?? 0001, 1, 1);
                        return;
                    }
                    
                    if (headerText.Contains("densità"))
                    {
                        string s = StringHelper.SanitizeString(tdText, true, false);
                        provinceToProcess.InhabitantsPerKm2 = StringHelper.ConvertToFloat(s);
                        return;
                    }

                    if (headerText.Equals("abitanti") || headerText.Equals("abitantiabitanti"))
                    {
                        string s = StringHelper.SanitizeString(tr.SelectSingleNode("td").FirstChild.InnerText, true, false);
                        provinceToProcess.Population = StringHelper.ConvertToInt(s);
                        return;
                    }

                    if (headerText.Contains("fuso orario"))
                    {
                        provinceToProcess.Timezone = StringHelper.SanitizeString(tdText, true, true);
                        return;
                    }

                    if (headerText.Contains("comuni"))
                    {
                        string s = StringHelper.SanitizeString(tdText, true, false);
                        provinceToProcess.ComuneCount = StringHelper.ConvertToInt(s);
                        return;
                    }

                    if (headerText.Contains("coordinate"))
                    {
                        float dataLat = StringHelper.ConvertToFloat(tr.SelectSingleNode("td")?.SelectSingleNode("a")?.Attributes["data-lat"].Value ?? "");
                        float dataLon = StringHelper.ConvertToFloat(tr.SelectSingleNode("td")?.SelectSingleNode("a")?.Attributes["data-lon"].Value ?? "");

                        provinceToProcess.Latitude = (decimal)dataLat;
                        provinceToProcess.Longitude = (decimal)dataLon;

                        return;
                    }

                    if (headerText.Equals("pil") || headerText.Equals("pilpil"))
                    {
                        string innerText = tdText;
                        int index = innerText.IndexOf('€');
                        innerText = innerText[0..index];
                        string s = StringHelper.SanitizeString(innerText, true, false);
                        provinceToProcess.GDPNominalMlnEuro = StringHelper.ConvertToFloat(s);
                        return;
                    }

                    if (headerText.Contains("pil procapite"))
                    {
                        string innerText = tdText;
                        int index = innerText.IndexOf('€');
                        innerText = innerText[0..index];
                        string s = StringHelper.SanitizeString(innerText, true, false);
                        provinceToProcess.GDPPerCapitaEuro = StringHelper.ConvertToFloat(s);
                        return;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error($"Error: {e.Message} - Province wiki page: {provinceWikiPagePath}");
                }
            });

            return provinceToProcess;
        }
    }
}
