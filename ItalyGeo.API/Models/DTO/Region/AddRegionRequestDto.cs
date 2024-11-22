using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ItalyGeo.API.Data;
using ItalyGeo.API.Models.Domain;

namespace ItalyGeo.API.Models.DTO.Region
{
    public class AddRegionRequestDto : IValidatableObject
    {
        [Required]
        public string Name { get; set; } = "";
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [Required]
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var _context = validationContext.GetService(typeof(ItalyGeoDbContext));
            if (_context != null && CapaluogoComuneId != null)
            {
                var italyGeoDbContext = (ItalyGeoDbContext)_context;
                var comune = italyGeoDbContext.Comunes.FirstOrDefault(x => x.Id == CapaluogoComuneId);
                if (comune == null) yield return new ValidationResult("CapaluogoComuneId not found.");
            }
        }
    }
}
