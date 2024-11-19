using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Comune
{
    class ComuneDto
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
        public string InhabitantName { get; set; } = "null";
        public string PublicHoliday { get; set; } = "null";
        public string PatronSaint { get; set; } = "null";
    }
}
