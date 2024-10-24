using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Region
{
    public class UpdateRegionRequest : IRegionRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string WikipediaPagePath { get; set; } = string.Empty;
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
