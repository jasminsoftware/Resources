using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    internal class SalesOrderTypeLineSerieResource
    {
        /// <summary>
        /// Gets or sets if is the default serie from this salesOrder type.
        /// </summary>
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the serie.
        /// </summary>
        [JsonProperty("serie")]
        public string Serie { get; set; }
    }
}
