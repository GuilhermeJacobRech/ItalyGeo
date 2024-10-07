using ItalyGeo.API.Models.DTO;
using ItalyGeo.API.Models.DTO.Auth;
using ItalyGeo.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ItalyGeo.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestDto authenticateRequestDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(authenticateRequestDto.Username.ToString()) ?? new IdentityUser();

                // Check if user exists and password is correct
                if (await _userManager.FindByNameAsync(authenticateRequestDto.Username.ToString()) != null && await _userManager.CheckPasswordAsync(user, authenticateRequestDto.Password))
                {
                    // Get user claims
                    var claims = await _userManager.GetClaimsAsync(user);

                    // Create Token
                    var jwtToken = _tokenRepository.CreateJWTToken(user, claims);

                    var response = new AuthenticateResponseDto
                    {
                        JwtToken = jwtToken,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(10)
                    };
                    return Ok(response);
                }

                return BadRequest("Username or password incorrect.");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
