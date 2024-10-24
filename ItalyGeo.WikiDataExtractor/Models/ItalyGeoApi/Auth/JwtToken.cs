using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WikiDataExtractor.Models.ItalyGeo.Auth
{
    public class JwtToken
    {
        [JsonProperty("jwtToken")]
        public string? AcessToken { get; set; }
        [JsonProperty("expiresAt")]
        public DateTime ExpiresAt { get; set; }
    }
}
