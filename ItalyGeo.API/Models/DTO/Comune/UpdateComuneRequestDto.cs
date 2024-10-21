using ItalyGeo.API.Data;
using System.ComponentModel.DataAnnotations;

namespace ItalyGeo.API.Models.DTO.Comune
{
    public class UpdateComuneRequestDto : IValidatableObject
    {
        [Required]
        public Guid ProvinceId { get; set; }
        [Required]
        public required string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [Required]
        public required string WikipediaPagePath { get; set; }
        public float AltitudeAboveSea { get; set; }
        public float AreaKm2 { get; set; }
        public int Population { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public string? ZipCode { get; set; }
        public string? Timezone { get; set; }
        public string? InhabitantName { get; set; }
        public string? PublicHoliday { get; set; }
        public string? PatronSaint { get; set; }

        // Validate if ProvinceId exists (taken from https://stackoverflow.com/a/53089588/10691380) 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var _context = validationContext.GetService(typeof(ItalyGeoDbContext));
            if (_context != null)
            {
                var italyGeoDbContext = (ItalyGeoDbContext)_context;
                var province = italyGeoDbContext.Provinces.FirstOrDefault(x => x.Id == ProvinceId);
                if (province == null) yield return new ValidationResult("RegionId not found.");
            }
        }
    }
}
