using ItalyGeo.API.Data;
using System.ComponentModel.DataAnnotations;

namespace ItalyGeo.API.Models.DTO.Province
{
    public class UpdateProvinceRequestDto : IValidatableObject
    {
        [Required]
        public Guid RegionId { get; set; }
        [Required]
        public required string Name { get; set; }
        public Guid? CapaluogoComuneId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [Required]
        public required string WikipediaPagePath { get; set; }
        public string? Acronym { get; set; }
        public int Population { get; set; }
        public float Areakm2 { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public int ComuneCount { get; set; }
        public string? ZipCode { get; set; }
        public string? Timezone { get; set; }
        [DataType(DataType.Date)]
        public DateTime YearCreated { get; set; }
        public float GDPNominalMlnEuro { get; set; }
        public float GDPPerCapitaEuro { get; set; }

        // Validate if RegionId exists (taken from https://stackoverflow.com/a/53089588/10691380) 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var _context = validationContext.GetService(typeof(ItalyGeoDbContext));
            if (_context != null)
            {
                var italyGeoDbContext = (ItalyGeoDbContext)_context;
                var region = italyGeoDbContext.Regions.FirstOrDefault(x => x.Id == RegionId);
                if (region == null) yield return new ValidationResult("RegionId not found.");
            }
        }
    }
}
