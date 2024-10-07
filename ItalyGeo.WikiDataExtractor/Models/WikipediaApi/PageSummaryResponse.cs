using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.WikipediaApi
{
    public class PageSummaryResponse
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("coordinates")]
        public Coordinate Coordinate { get; set; } = new Coordinate();
    }
}
