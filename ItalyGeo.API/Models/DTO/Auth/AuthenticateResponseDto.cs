namespace ItalyGeo.API.Models.DTO.Auth
{
    public class AuthenticateResponseDto
    {
        public required string JwtToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
