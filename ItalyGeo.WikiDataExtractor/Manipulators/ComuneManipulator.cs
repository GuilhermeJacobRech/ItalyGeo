﻿using HtmlAgilityPack;
using WikiDataExtractor.Helpers;
using WikiDataExtractor.Models.ItalyGeo.Comune;
using WikiDataExtractor.Services;
using Serilog;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Comune;
using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Province;
using WikiDataExtractor.Models.ItalyGeo.Province;
using WikiDataExtractor.Models.ItalyGeo.Region;

namespace WikiDataExtractor.Manipulators
{
    public class ComuneManipulator
    {
        private readonly IWikipediaApi _wikipediaApi;
        private readonly IItalyGeoApi _italyGeoApi;
        private readonly ILogger _logger;

        public ComuneManipulator(IWikipediaApi wikipediaApi,
            IItalyGeoApi italyGeoApi,
            ILogger logger)
        {
            _wikipediaApi = wikipediaApi;
            _italyGeoApi = italyGeoApi;
            this._logger = logger;
        }

        public async IAsyncEnumerable<List<IComuneRequest>> ParseHtmlAsync(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // XPath to the unordered list. This unordered list is ordered (yeah, i know) alphabetically
            string xpath = $"/html/body/section[2]/div/div/ul";
            var ul = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            // Each li is a link to a page that lists every comune where the name starts with the respective letter
            var listItems = ul.SelectNodes(".//li");

            foreach (var li in listItems)
            {
                await foreach (var comunesByLetter in ParseListItemAsync(li))
                {
                    yield return comunesByLetter;
                }
            }
        }

        private async IAsyncEnumerable<List<IComuneRequest>> ParseListItemAsync(HtmlNode li)
        {
            var nodeA = li.SelectSingleNode("a");
            var comunesByLetterWikiPath = StringHelper.SanitizeString(nodeA.Attributes["href"].Value, false, true);
            var html = await _wikipediaApi.GetPageHtmlAsync(comunesByLetterWikiPath);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            string xpath = $"//table";

            var comunesByLetterTable = htmlDoc.DocumentNode.SelectNodes(xpath).ElementAt(0);

            // Table Rows, skip header
            var trNodes = comunesByLetterTable.SelectNodes(".//tr").Skip(1);

            if (comunesByLetterWikiPath == "Comuni_d'Italia_(H-J)")
            {
                var trNodesI = htmlDoc.DocumentNode.SelectNodes(xpath).ElementAt(1).SelectNodes(".//tr").Skip(1);
                var trNodesJ = htmlDoc.DocumentNode.SelectNodes(xpath).ElementAt(2).SelectNodes(".//tr").Skip(1);

                trNodes = trNodes.Concat(trNodesI);
                trNodes = trNodes.Concat(trNodesJ);
            }

            var comunesToProcess = new List<IComuneRequest>();

            await Parallel.ForEachAsync(trNodes, new ParallelOptions() { MaxDegreeOfParallelism = 20 }, async (tr, ct) =>
            {
                var comuneToProcess = await ParseTableRowAsync(tr);
                if (comuneToProcess != null) 
                {
                    comunesToProcess.Add(comuneToProcess);
                } 
            });

            yield return comunesToProcess;
        }

        private async Task<IComuneRequest?> ParseTableRowAsync(HtmlNode tr)
        {
            // Data cells in the current table row
            var tdNodes = tr.SelectNodes(".//td");

            // End of table
            if (tdNodes == null)
            {
                _logger.Error($"tDNodes null - Inner html: {tr.InnerHtml}");
                return null;
            }

            // Node that contains name and Wikipedia page path of current Comune
            var node0a = tdNodes.ElementAt(0).SelectSingleNode("a");

            string comuneWikiPagePath = StringHelper.SanitizeString(node0a.Attributes["href"].Value, false, true);
            if (comuneWikiPagePath == "Pagani") comuneWikiPagePath = "Pagani_(Italia)";
            string comuneName = StringHelper.SanitizeString(node0a.InnerText, false, true);

            // Node that contains Wikipedia page path of the Province that the current Comune belongs to
            var node1a = tdNodes.ElementAt(1).SelectSingleNode("a");
            string provinceWikiPagePath = StringHelper.SanitizeString(node1a.Attributes["href"].Value, false, true);

            // Check if Province exists
            var provinceResponse = await _italyGeoApi.GetProvinceByWikiPagePathAsync(provinceWikiPagePath);
            if (provinceResponse == null)
            {
                string altProvinceWikiPagePath = provinceWikiPagePath.Replace("Provincia_di_", "Città_metropolitana_di_");
                provinceResponse = await _italyGeoApi.GetProvinceByWikiPagePathAsync(altProvinceWikiPagePath);

                if (provinceResponse == null)
                {
                    altProvinceWikiPagePath = provinceWikiPagePath.Replace("Provincia_di_", "Libero_consorzio_comunale_di_");
                    provinceResponse = await _italyGeoApi.GetProvinceByWikiPagePathAsync(altProvinceWikiPagePath);

                    if (provinceResponse == null)
                    {
                        altProvinceWikiPagePath = provinceWikiPagePath.Replace("Provincia_di_Roma", "Città_metropolitana_di_Roma_Capitale");
                        provinceResponse = await _italyGeoApi.GetProvinceByWikiPagePathAsync(altProvinceWikiPagePath);

                        if (provinceResponse == null)
                        {
                            altProvinceWikiPagePath = provinceWikiPagePath.Replace("Provincia_di_Carbonia-Iglesias", "Provincia_del_Sud_Sardegna");
                            provinceResponse = await _italyGeoApi.GetProvinceByWikiPagePathAsync(altProvinceWikiPagePath);

                            if (provinceResponse == null)
                            {
                                altProvinceWikiPagePath = provinceWikiPagePath.Replace("Provincia_del_Medio_Campidano", "Provincia_del_Sud_Sardegna");
                                provinceResponse = await _italyGeoApi.GetProvinceByWikiPagePathAsync(altProvinceWikiPagePath);

                                if (provinceResponse == null)
                                {
                                    altProvinceWikiPagePath = provinceWikiPagePath.Replace("Aosta", "Valle_d'Aosta");
                                    provinceResponse = await _italyGeoApi.GetProvinceByWikiPagePathAsync(altProvinceWikiPagePath);

                                    if (provinceResponse == null)
                                    {
                                        _logger.Error($"Could not found province {provinceWikiPagePath} when trying to process comune {comuneWikiPagePath}");
                                        return null;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Get Comune on ItalyGeo
            var comuneItalyGeoResponse = await _italyGeoApi.GetComuneByWikiPagePathAsync(comuneWikiPagePath);
            ComuneDto? comuneDto = await ParseComuneWikiPage(comuneWikiPagePath);

            if (comuneItalyGeoResponse != null && comuneDto != null)
            {
                return new UpdateComuneRequest
                {
                    Id = comuneItalyGeoResponse.Id,
                    ProvinceId = provinceResponse.Id,
                    Name = comuneName,
                    WikipediaPagePath = comuneWikiPagePath,
                    AreaKm2 = comuneDto.AreaKm2,
                    InhabitantsPerKm2 = comuneDto.InhabitantsPerKm2,
                    Latitude = comuneDto.Latitude,
                    Longitude = comuneDto.Longitude,
                    Population = comuneDto.Population,
                    Timezone = comuneDto.Timezone,
                    AltitudeAboveSeaMeterMSL = comuneDto.AltitudeAboveSeaMeterMSL,
                    ZipCode = comuneDto.ZipCode
                };
            }
            if (comuneItalyGeoResponse == null && comuneDto != null)
            {
                return new AddComuneRequest
                {
                    ProvinceId = provinceResponse.Id,
                    Name = comuneName,
                    WikipediaPagePath = comuneWikiPagePath,
                    AreaKm2 = comuneDto.AreaKm2,
                    InhabitantsPerKm2 = comuneDto.InhabitantsPerKm2,
                    Latitude = comuneDto.Latitude,
                    Longitude = comuneDto.Longitude,
                    Population = comuneDto.Population,
                    Timezone = comuneDto.Timezone,
                    AltitudeAboveSeaMeterMSL = comuneDto.AltitudeAboveSeaMeterMSL,
                    ZipCode = comuneDto.ZipCode
                };
            }
            return null;
        }

        private async Task<ComuneDto?> ParseComuneWikiPage(string comuneWikiPagePath)
        {
            var comuneWikiPageHTML = await _wikipediaApi.GetPageHtmlAsync(comuneWikiPagePath);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(comuneWikiPageHTML);

            string xpath = "html/body/section[1]/table/tbody/tr";
            var infoboxTableRows = htmlDoc.DocumentNode.SelectNodes(xpath);
            if (infoboxTableRows == null)
            {
                _logger.Error($"Could not find infobox - Comune wiki page: {comuneWikiPagePath}");
                return null;
            }

            var comuneToProcess = new ComuneDto();

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

                    if (headerText.Equals("cod postale") || headerText.Equals("cod postalecod postale"))
                    {
                        comuneToProcess.ZipCode = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, true);
                        return;
                    }

                    if (headerText.Contains("altitudine"))
                    {
                        string s = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, false);
                        comuneToProcess.AltitudeAboveSeaMeterMSL = StringHelper.ConvertToFloat(s);
                        return;
                    }

                    if (headerText.Contains("superficie"))
                    {
                        string s = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, false);
                        comuneToProcess.AreaKm2 = StringHelper.ConvertToFloat(s);
                        return;
                    }

                    if (headerText.Contains("densità"))
                    {
                        string s = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, false);
                        comuneToProcess.InhabitantsPerKm2 = StringHelper.ConvertToFloat(s);
                        return;
                    }

                    if (headerText.Equals("abitanti") || headerText.Equals("abitantiabitanti"))
                    {
                        string s = StringHelper.SanitizeString(tr.SelectSingleNode("td").FirstChild.InnerText, true, false);
                        comuneToProcess.Population = StringHelper.ConvertToInt(s);
                        return;
                    }

                    if (headerText.Contains("fuso orario"))
                    {
                        comuneToProcess.Timezone = StringHelper.SanitizeString(tr.SelectSingleNode("td").InnerText, true, true);
                        return;
                    }

                    if (headerText.Contains("coordinate"))
                    {
                        float dataLat = StringHelper.ConvertToFloat(tr.SelectSingleNode("td")?.SelectSingleNode("a")?.Attributes["data-lat"].Value ?? "");
                        float dataLon = StringHelper.ConvertToFloat(tr.SelectSingleNode("td")?.SelectSingleNode("a")?.Attributes["data-lon"].Value ?? "");

                        comuneToProcess.Latitude = (decimal)dataLat;
                        comuneToProcess.Longitude = (decimal)dataLon;

                        return;
                    }
                }
                catch(NullReferenceException e)
                {
                    _logger.Error($"Error: {e.Message} - Comune wiki page: {comuneWikiPagePath}");
                }
            });

            return comuneToProcess;
        }
    }
}
