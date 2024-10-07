using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItalyGeo.API.Models.Domain
{
    public class Comune
    {
        public Guid Id { get; set; }
        public Guid ProvinceId { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
        public decimal Longitude { get; set; }

        public string WikipediaPagePath { get; set; }

        // Navigation properties
        public Province Province { get; set; }
    }
}
