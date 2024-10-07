using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Province
{
    public class AddProvinceRequest
    {
        public string Name { get; set; }
        public Guid RegionId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string WikipediaPagePath { get; set; }
        public string Acronym { get; set; }
        public int Population { get; set; }
        public float Areakm2 { get; set; }
        public float InhabitantsPerKm2 { get; set; }
        public int ComuneCount { get; set; }

        [DataType(DataType.Date)]
        public DateTime yearCreated { get; set; }
    }
}
