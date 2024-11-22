using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Comune;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.ItalyGeo.Comune
{
    public class AddComuneRequest : IComuneRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProvinceId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string WikipediaPagePath { get; set; } = string.Empty;
        public float AltitudeAboveSeaMeterMSL { get; set; }
        public float AreaKm2 { get; set; }
        public int Population { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public string ZipCode { get; set; } = "null";
        public string Timezone { get; set; } = "null";
    }
}
