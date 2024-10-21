using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Province
{
    public class ProvinceDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string WikipediaPagePath { get; set; } = string.Empty;
        public string? Acronym { get; set; }
        public int Population { get; set; }
        public float AreaKm2 { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public int ComuneCount { get; set; }
        public string? Zipcode { get; set; }
        public string? Timezone { get; set; }

        [DataType(DataType.Date)]
        public DateTime yearCreated { get; set; }
        public float GDPNominalMlnEuro { get; set; }
        public float GDPPerCapitaEuro { get; set; }
    }
}
