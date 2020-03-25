using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    /// <summary>
    /// Describes a money resource.
    /// </summary>
    internal class MoneyResource
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [JsonProperty("amount")]
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        [JsonProperty("symbol")]
        public string Currency { get; set; }

        #endregion
    }
}