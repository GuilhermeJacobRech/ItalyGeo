using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.WikipediaApi
{
    public class Coordinate
    {
        [Column(TypeName = "decimal(12, 9)")]
        [JsonPropertyName("lat")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
        [JsonPropertyName("lon")]
        public decimal Longitude { get; set; }
    }
}
