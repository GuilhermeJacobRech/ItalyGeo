using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.ItalianCitizenshipTrackerApi.Region
{
    public class RegionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public Uri WikipediaPagePath { get; set; }
    }
}
