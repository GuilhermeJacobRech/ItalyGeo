using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItalyGeo.API.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
        public decimal Longitude { get; set; }
        public required string WikipediaPagePath { get; set; }

        public int Population { get; set; }
        public float AreaKm2 { get; set; }
        public string? Acronym { get; set; }
        public float InhabitantsPerKm2 { get; set; }

        public int ComuneCount { get; set; }

        public int ProvinceCount { get; set; }
        public string? Timezone { get; set; }
        public string? InhabitantName { get; set; }
        public string? PatronSaint { get; set; }
        public float GDPNominalMlnEuro { get; set; }
        public float GDPPerCapitaEuro { get; set; }
    }
}
