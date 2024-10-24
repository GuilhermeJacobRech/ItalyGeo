using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.ItalyGeo.Region
{
    public class RegionResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string WikipediaPagePath { get; set; }
    }
}
