using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.ItalyGeo.Comune
{
    public class ComuneResponse
    {
        public Guid Id { get; set; }
        public Guid ProvinceId { get; set; }
        public required string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public required string WikipediaPagePath { get; set; }
        public float AltitudeAboveSeaMeterMSL { get; set; }
        public float AreaKm2 { get; set; }
        public int Population { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public string? ZipCode { get; set; }
        public string? Timezone { get; set; }
        public string? InhabitantName { get; set; }
        public string? PublicHoliday { get; set; }
        public string? PatronSaint { get; set; }
    }
}
