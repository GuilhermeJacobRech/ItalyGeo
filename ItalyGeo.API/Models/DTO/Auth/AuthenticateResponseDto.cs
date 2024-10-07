namespace ItalyGeo.API.Models.DTO.Auth
{
    public class AuthenticateResponseDto
    {
        public string JwtToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
