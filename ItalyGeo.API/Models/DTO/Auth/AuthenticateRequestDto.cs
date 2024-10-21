using System.ComponentModel.DataAnnotations;

namespace ItalyGeo.API.Models.DTO.Auth
{
    public class AuthenticateRequestDto
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
