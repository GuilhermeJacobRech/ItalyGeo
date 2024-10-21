using ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Province;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Province
{
    public class AddProvinceRequest : IProvinceRequest
    {
        public string WikipediaPagePath { get; set; }
        public string Name { get; set; }
        public Guid RegionId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Acronym { get; set; }
        public int Population { get; set; }
        public float AreaKm2 { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public int ComuneCount { get; set; }
        public string? Zipcode { get; set; }
        public string? Timezone { get; set; }

        [DataType(DataType.Date)]
        public DateTime YearCreated { get; set; }
        public float GDPNominalMlnEuro { get; set; }
        public float GDPPerCapitaEuro { get; set; }
    }
}
