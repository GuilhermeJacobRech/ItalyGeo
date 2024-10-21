using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItalyGeo.API.Models.Domain
{
    public class Province
    {
        public Guid Id { get; set; }
        public Guid RegionId { get; set; }
        public required string Name { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
        public decimal Longitude { get; set; }
        public required string WikipediaPagePath { get; set; }

        public string? Acronym { get; set; }
        public int Population { get; set; }
        public float Areakm2 { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public int ComuneCount { get; set; }

        public string? Zipcode { get; set; }
        public string? Timezone { get; set; }

        [DataType(DataType.Date)]
        public DateTime YearCreated { get; set; }
        public float GDPNominalMlnEuro { get; set; }
        public float GDPPerCapitaEuro { get; set; }

        // Navigation properties
        public required Region Region { get; set; }
    }
}
