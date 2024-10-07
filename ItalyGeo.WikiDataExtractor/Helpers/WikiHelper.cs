﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WikiDataExtractor.Helpers
{
    public class WikiHelper
    {
        public static string BaseWikipediaUrl { get { return "it.wikipedia.org/wiki/"; } }
        public static string RegionsOfItalyPagePath { get { return "Regioni_d'Italia"; } }
        public static string ProvincesOfItalyPagePath { get { return "Province_d'Italia"; } }
        public static string ComunesOfItalyPagePath { get { return "Comuni_d'Italia"; } }

    }
}
