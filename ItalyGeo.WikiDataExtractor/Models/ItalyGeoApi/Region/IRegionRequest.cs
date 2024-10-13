using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Region
{
    public interface IRegionRequest
    {
        string WikipediaPagePath { get; set; }
    }
}
