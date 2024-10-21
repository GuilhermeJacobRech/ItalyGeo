﻿using System.ComponentModel.DataAnnotations;

namespace ItalyGeo.API.Models.DTO.Region
{
    public class UpdateRegionRequestDto
    {
        [Required]
        public string Name { get; set; } = "";
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [Required]
        public string WikipediaPagePath { get; set; } = "";
        public Guid? CapaluogoComuneId { get; set; }
        public int Population { get; set; }
        public float Areakm2 { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public int ComuneCount { get; set; }
        public int ProvinceCount { get; set; }
        public string? Acronym { get; set; }
        public string? Timezone { get; set; }
        public string? InhabitantName { get; set; }
        public string? PatronSaint { get; set; }
        public float GDPNominalMlnEuro { get; set; }
        public float GDPPerCapitaEuro { get; set; }
    }
}
