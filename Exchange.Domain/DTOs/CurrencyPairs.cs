using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Exchange.Domain.DTOs
{
    public class CurrencyPairs
    {
        [JsonPropertyName("base_code")]
        public string BaseCode { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
