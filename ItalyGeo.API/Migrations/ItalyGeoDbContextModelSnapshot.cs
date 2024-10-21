﻿// <auto-generated />
using System;
using ItalyGeo.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ItalyGeo.API.Migrations
{
    [DbContext(typeof(ItalyGeoDbContext))]
    partial class ItalyGeoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ItalyGeo.API.Models.Domain.Comune", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("AltitudeAboveSeaMeterMSL")
                        .HasColumnType("real");

                    b.Property<float>("AreaKm2")
                        .HasColumnType("real");

                    b.Property<string>("InhabitantName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("InhabitantsPerKm2")
                        .HasColumnType("real");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(12, 9)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(12, 9)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatronSaint")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Population")
                        .HasColumnType("int");

                    b.Property<Guid>("ProvinceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PublicHoliday")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Timezone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WikipediaPagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.HasIndex("WikipediaPagePath")
                        .IsUnique();

                    b.ToTable("Comunes");
                });

            modelBuilder.Entity("ItalyGeo.API.Models.Domain.Province", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Acronym")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Areakm2")
                        .HasColumnType("real");

                    b.Property<Guid?>("CapaluogoComuneId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ComuneCount")
                        .HasColumnType("int");

                    b.Property<float>("GDPNominalMlnEuro")
                        .HasColumnType("real");

                    b.Property<float>("GDPPerCapitaEuro")
                        .HasColumnType("real");

                    b.Property<float>("InhabitantsPerKm2")
                        .HasColumnType("real");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(12, 9)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(12, 9)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Population")
                        .HasColumnType("int");

                    b.Property<Guid>("RegionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Timezone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WikipediaPagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("YearCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Zipcode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CapaluogoComuneId");

                    b.HasIndex("RegionId");

                    b.HasIndex("WikipediaPagePath")
                        .IsUnique();

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("ItalyGeo.API.Models.Domain.Region", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Acronym")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("AreaKm2")
                        .HasColumnType("real");

                    b.Property<Guid?>("CapaluogoComuneId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ComuneCount")
                        .HasColumnType("int");

                    b.Property<float>("GDPNominalMlnEuro")
                        .HasColumnType("real");

                    b.Property<float>("GDPPerCapitaEuro")
                        .HasColumnType("real");

                    b.Property<string>("InhabitantName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("InhabitantsPerKm2")
                        .HasColumnType("real");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(12, 9)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(12, 9)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatronSaint")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Population")
                        .HasColumnType("int");

                    b.Property<int>("ProvinceCount")
                        .HasColumnType("int");

                    b.Property<string>("Timezone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WikipediaPagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CapaluogoComuneId");

                    b.HasIndex("WikipediaPagePath")
                        .IsUnique();

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("ItalyGeo.API.Models.Domain.Comune", b =>
                {
                    b.HasOne("ItalyGeo.API.Models.Domain.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("ItalyGeo.API.Models.Domain.Province", b =>
                {
                    b.HasOne("ItalyGeo.API.Models.Domain.Comune", "CapaluogoComune")
                        .WithMany()
                        .HasForeignKey("CapaluogoComuneId");

                    b.HasOne("ItalyGeo.API.Models.Domain.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CapaluogoComune");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("ItalyGeo.API.Models.Domain.Region", b =>
                {
                    b.HasOne("ItalyGeo.API.Models.Domain.Comune", "CapaluogoComune")
                        .WithMany()
                        .HasForeignKey("CapaluogoComuneId");

                    b.Navigation("CapaluogoComune");
                });
#pragma warning restore 612, 618
        }
    }
}
