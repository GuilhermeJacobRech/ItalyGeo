using ItalyGeo.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ItalyGeo.API.Data
{
    public class ItalyGeoAuthDbContext : IdentityDbContext
    {
        public ItalyGeoAuthDbContext(DbContextOptions<ItalyGeoAuthDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<IdentityUser>();
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "E36F36B3-8A17-40C2-9C52-580864E6CB3B",
                UserName = "admin",
                NormalizedUserName = "admin",
                PasswordHash = hasher.HashPassword(null, "password"),
                SecurityStamp = string.Empty,
            });


            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(new IdentityUserClaim<string>
            {
                Id = 1,
                UserId = "E36F36B3-8A17-40C2-9C52-580864E6CB3B",
                ClaimType = "Admin",
                ClaimValue = "True"
            });
        }
    }
}
