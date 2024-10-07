using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ItalyGeo.API.Models.DTO.Region;

namespace ItalyGeo.API.Models.DTO.Province
{
    public class ProvinceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RegionId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string WikipediaPagePath { get; set; }
        public string Acronym { get; set; }
        public int Population { get; set; }
        public float Areakm2 { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public int ComuneCount { get; set; }

        [DataType(DataType.Date)]
        public DateTime yearCreated { get; set; }

        // Navigation properties
        [JsonPropertyName("region")]
        public RegionDto RegionDto { get; set; }
    }
}
