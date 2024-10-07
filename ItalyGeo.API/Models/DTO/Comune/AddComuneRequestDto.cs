using ItalyGeo.API.Data;
using ItalyGeo.API.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace ItalyGeo.API.Models.DTO.Comune
{
    public class AddComuneRequestDto : IValidatableObject
    {
        [Required]
        public Guid ProvinceId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public string WikipediaPagePath { get; set; }

        // Validate if ProvinceId exists (taken from https://stackoverflow.com/a/53089588/10691380) 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var _context = (ItalyGeoDbContext)validationContext.GetService(typeof(ItalyGeoDbContext));
            if (_context != null)
            {
                var province = _context.Provinces.FirstOrDefault(x => x.Id == ProvinceId);
                if (province == null) yield return new ValidationResult("ProvinceId not found.");
            }
        }
    }
}
