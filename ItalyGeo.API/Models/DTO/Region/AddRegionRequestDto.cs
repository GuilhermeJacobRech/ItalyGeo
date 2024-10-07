using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ItalyGeo.API.Models.DTO.Region
{
    public class AddRegionRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public string WikipediaPagePath { get; set; }
        [Required]
        public int Population { get; set; }
        [Required]
        public float Areakm2 { get; set; }
        [Required]
        public float InhabitantsPerKm2 { get; set; }
        [Required]
        public int ComuneCount { get; set; }
        [Required]
        public int ProvinceCount { get; set; }
    }
}
