using ItalyGeo.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ItalyGeo.API.Data
{
    public class ItalyGeoAuthDbContext : IdentityDbContext
    {
        private readonly IConfiguration _configuration;

        public ItalyGeoAuthDbContext(DbContextOptions<ItalyGeoAuthDbContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<IdentityUser>();
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = _configuration["Admin:id"],
                UserName = "admin",
                NormalizedUserName = "admin",
                PasswordHash = hasher.HashPassword(null, _configuration["Admin:password"]),
                SecurityStamp = string.Empty,
            });

            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(new IdentityUserClaim<string>
            {
                Id = 1,
                UserId = _configuration["Admin:password"],
                ClaimType = "Admin",
                ClaimValue = "True"
            });
        }
    }
}
