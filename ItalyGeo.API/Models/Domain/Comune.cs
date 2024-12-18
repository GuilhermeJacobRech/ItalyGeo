﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace ItalyGeo.API.Models.Domain
{
    public class Comune
    {
        public Guid Id { get; set; }
        public Guid ProvinceId { get; set; }
        public required string Name { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(12, 9)")]
        public decimal Longitude { get; set; }

        public required string WikipediaPagePath { get; set; }
        public float AltitudeAboveSeaMeterMSL { get; set; }
        public float AreaKm2 { get; set; }
        public int Population { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public string? ZipCode { get; set; }
        public string? Timezone { get; set; }

        // Navigation properties
        public required Province Province { get; set; }
    }
}
