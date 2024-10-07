using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WikiDataExtractor.Helpers;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Comune;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Province;
using WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Region;
using WikiDataExtractor.Services;
using Serilog;

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

        public async Task<List<AddComuneRequest>> ParseHtmlAsync(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // XPath to the unsorted list of links to comune information by alphabetical sections
            string xpath = $"/html/body/section[2]/div/div/ul";
            var comunesUl = htmlDoc.DocumentNode.SelectSingleNode(xpath);
            // Each list item (li) contains a link to a page that contains every comune where the name starts with a specific letter
            var listItems = comunesUl.SelectNodes(".//li");
            var comunesToAdd = new List<AddComuneRequest>();

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 20
            };

            // For each table row in the HTML table that contains the comune data
            await Parallel.ForEachAsync(listItems, options, async (li, ct) =>
            {
                comunesToAdd.AddRange(await ParseListItemAsync(li));
            });

            /*foreach (var li in listItems)
            {
                comunesToAdd.AddRange(await ParseListItemAsync(li));
            }*/

            return comunesToAdd;
        }

        private async Task<List<AddComuneRequest>> ParseListItemAsync(HtmlNode li)
        {
            var aNode = li.SelectSingleNode("a");
            var comunesByLetterWikiPath = StringHelper.SanitizeString(aNode.Attributes["href"].Value);
            var html = await _wikipediaApi.GetPageHtmlAsync(comunesByLetterWikiPath);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            string xpath = $"//table";

            var comunesByLetterTable = htmlDoc.DocumentNode.SelectNodes(xpath).ElementAt(0);

            // Table Rows, skip header
            var trNodes = comunesByLetterTable.SelectNodes(".//tr").Skip(1);

            var comunesToAdd = new List<AddComuneRequest>();

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 20
            };

            // For each table row in the HTML table that contains the comune data
            await Parallel.ForEachAsync(trNodes, options, async (tr, ct) =>
            {
                var comuneToAdd = await ParseTableRowAsync(tr);
                if (comuneToAdd != null) comunesToAdd.Add(comuneToAdd);
            });

            return comunesToAdd;
        }

        private async Task<AddComuneRequest?> ParseTableRowAsync(HtmlNode tr)
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
            var aNodeZero = tdNodes.ElementAt(0).SelectSingleNode("a");

            string comuneWikiPagePath = StringHelper.SanitizeString(aNodeZero.Attributes["href"].Value);
            string comuneName = StringHelper.SanitizeString(aNodeZero.InnerText);

            // Node that contains Wikipedia page path of the Province that the current Comune belongs to
            var aNodeOne = tdNodes.ElementAt(1).SelectSingleNode("a");
            string provinceWikiPagePath = StringHelper.SanitizeString(aNodeOne.Attributes["href"].Value);

            // Check if Comune already exists
            var comuneResponse = await _italyGeoApi.GetComuneByWikiPagePathAsync(comuneWikiPagePath);
            if (comuneResponse != null) return null;

            // Check if Province exists (it should)
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
                                        _logger.Error($"Could not find province - Comune name: {comuneName} - Prov wiki page: {provinceWikiPagePath}");
                                        return null;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Get comune summary
            var comuneSummary = await _wikipediaApi.GetPageSummaryAsync(comuneWikiPagePath) ?? new();

            return new AddComuneRequest
            {
                Name = comuneName,
                ProvinceId = provinceResponse.Id,
                Latitude = comuneSummary.Coordinate.Latitude,
                Longitude = comuneSummary.Coordinate.Longitude,
                WikipediaPagePath = comuneWikiPagePath
            };
        }
    }
}
