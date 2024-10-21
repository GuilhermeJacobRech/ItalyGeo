using ItalyGeo.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serilog;
using System.Reflection.Metadata;
using System.Xml;

namespace ItalyGeo.API.Data
{
    public class ItalyGeoDbContext : DbContext
    {
        public ItalyGeoDbContext(DbContextOptions<ItalyGeoDbContext> dbContextOptions): base(dbContextOptions)
        {
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
                .IsRequired(false);

            // One-to-one relationship for the capital Comune in Province
            modelBuilder.Entity<Province>()
                .HasOne(r => r.CapaluogoComune)
                .WithMany()
                .HasForeignKey(r => r.CapaluogoComuneId)
                .IsRequired(false);
        }

        public DbSet<Comune> Comunes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Region> Regions { get; set; }
        
    }
}
