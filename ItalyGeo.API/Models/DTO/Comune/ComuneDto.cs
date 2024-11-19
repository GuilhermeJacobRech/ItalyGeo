using ItalyGeo.API.Models.Domain;
using ItalyGeo.API.Models.DTO.Province;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ItalyGeo.API.Models.DTO.Comune
{
    public class ComuneDto
    {
        public Guid Id { get; set; }
        public Guid ProvinceId { get; set; }
        public required string Name { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
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

        // Navigation properties
        [JsonPropertyName("province")]
        public ProvinceDto ProvinceDto { get; set; }

    }
}
