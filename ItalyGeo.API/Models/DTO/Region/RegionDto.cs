using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItalyGeo.API.Models.DTO.Region
{
    public class RegionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string WikipediaPagePath { get; set; } = "";
        public Guid? CapaluogoComuneId { get; set; }
        public int Population { get; set; }
        public float Areakm2 { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public int ComuneCount { get; set; }
        public int ProvinceCount { get; set; }
        public string? Acronym { get; set; }
        public string? Timezone { get; set; }
        public float GDPNominalMlnEuro { get; set; }
        public float GDPPerCapitaEuro { get; set; }
    }
}
