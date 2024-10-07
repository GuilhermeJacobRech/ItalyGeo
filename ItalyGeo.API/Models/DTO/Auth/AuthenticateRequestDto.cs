using System.ComponentModel.DataAnnotations;

namespace ItalyGeo.API.Models.DTO.Auth
{
    public class AuthenticateRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
