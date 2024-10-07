using ItalyGeo.API.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItalyGeo.API.Models.DTO.Province
{
    public class AddProvinceRequestDto : IValidatableObject
    {
        [Required]
        public Guid RegionId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public string WikipediaPagePath { get; set; }
        [Required]
        public string Acronym { get; set; }
        [Required]
        public int Population { get; set; }
        [Required]
        public float Areakm2 { get; set; }
        [Required]
        public float InhabitantsPerKm2 { get; set; }
        [Required]
        public int ComuneCount { get; set; }
        [Required]

        [DataType(DataType.Date)]
        public DateTime yearCreated { get; set; }

        // Validate if RegionId exists (taken from https://stackoverflow.com/a/53089588/10691380) 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var _context = (ItalyGeoDbContext)validationContext.GetService(typeof(ItalyGeoDbContext));
            if (_context != null)
            {
                var region = _context.Regions.FirstOrDefault(x => x.Id == RegionId);
                if (region == null) yield return new ValidationResult("RegionId not found.");
            }
        }
    }
}
