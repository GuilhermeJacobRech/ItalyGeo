using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Comune
{
    public class AddComuneRequest
    {
        public string Name { get; set; }
        public Guid ProvinceId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string WikipediaPagePath { get; set; }
        public float AltitudeAboveSea { get; set; }
        public float AreaKm2 { get; set; }
        public int Population { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public string ZipCode { get; set; } = "null";
        public string Timezone { get; set; } = "null";
        public string InhabitantName { get; set; } = "null";
        public string PublicHoliday { get; set; } = "null";
        public string PatronSaint { get; set; } = "null";
    }
}
