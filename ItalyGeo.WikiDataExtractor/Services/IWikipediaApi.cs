using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiDataExtractor.Models.WikipediaApi;

namespace WikiDataExtractor.Services
{
    public interface IWikipediaApi
    {
        Task<PageSummaryResponse?> GetPageSummaryAsync(string pageTitle);
        Task<string?> GetPageHtmlAsync(string pageTitle);
    }
}
