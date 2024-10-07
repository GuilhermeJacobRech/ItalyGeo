using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ItalyGeo.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, IEnumerable<Claim> claims);   
    }
}
