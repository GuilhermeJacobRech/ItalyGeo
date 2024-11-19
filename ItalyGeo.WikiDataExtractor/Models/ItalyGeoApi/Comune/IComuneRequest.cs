using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalyGeo.WikiDataExtractor.Models.ItalyGeoApi.Comune
{
    public interface IComuneRequest
    {
        string WikipediaPagePath { get; set; }
    }
}
