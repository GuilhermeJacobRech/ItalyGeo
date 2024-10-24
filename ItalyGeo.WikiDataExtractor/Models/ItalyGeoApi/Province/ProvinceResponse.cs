using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.ItalyGeo.Province
{
    public class ProvinceResponse
    {
        public Guid Id { get; set; }
        public Guid RegionId { get; set; }
        public string Name { get; set; }
        public string WikipediaPagePath { get; set; }
    }
}
