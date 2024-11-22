using ItalyGeo.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serilog;
using System.Reflection.Metadata;
using System.Xml;

namespace ItalyGeo.API.Data
{
    public class ItalyGeoDbContext : IdentityDbContext
    {
        private readonly IConfiguration _configuration;

        public ItalyGeoDbContext(DbContextOptions<ItalyGeoDbContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set column WikipediaPagePath from table Regions as unique
            modelBuilder.Entity<Region>()
                    .HasIndex(e => e.WikipediaPagePath)
                    .IsUnique();

            // Set column WikipediaPagePath from table Province as unique
            modelBuilder.Entity<Province>()
                    .HasIndex(e => e.WikipediaPagePath)
                    .IsUnique();

            // Set column WikipediaPagePath from table Comune as unique
            modelBuilder.Entity<Comune>()
                    .HasIndex(e => e.WikipediaPagePath)
                    .IsUnique();

            // One-to-one relationship for the capital Comune in Region
            modelBuilder.Entity<Region>()
                .HasOne(r => r.CapaluogoComune) 
                .WithMany()
                .HasForeignKey(r => r.CapaluogoComuneId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // One-to-one relationship for the capital Comune in Province
            modelBuilder.Entity<Province>()
                .HasOne(r => r.CapaluogoComune)
                .WithMany()
                .HasForeignKey(r => r.CapaluogoComuneId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

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
                UserId = _configuration["Admin:id"],
                ClaimType = "Admin",
                ClaimValue = "True"
            });
        }

        public DbSet<Comune> Comunes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Region> Regions { get; set; }
        
    }
}
