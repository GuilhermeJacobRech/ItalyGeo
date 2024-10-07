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
        public string Name { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        // Navigation properties
        [JsonPropertyName("province")]
        public ProvinceDto ProvinceDto { get; set; }
    }
}
