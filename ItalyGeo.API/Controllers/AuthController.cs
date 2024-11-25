using ItalyGeo.API.Models.DTO;
using ItalyGeo.API.Models.DTO.Auth;
using ItalyGeo.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthenticateResponseDto))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "If username/password does not match.")]
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

                // Returns Unauthorized as per https://stackoverflow.com/a/32752617/10691380
                return Unauthorized("Username or password incorrect.");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
