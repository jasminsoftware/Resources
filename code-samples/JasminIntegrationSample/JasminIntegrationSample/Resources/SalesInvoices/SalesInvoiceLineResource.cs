using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    internal class SalesInvoiceLineResource
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the line identifier.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the line Index.
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("salesItem")]
        public string Item { get; set; }

        [JsonProperty("warehouse")]
        public string Warehouse { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        public double? Quantity { get; set; }

        [JsonProperty("unitPrice")]
        public MoneyResource Price { get; set; }

        #endregion
    }
}
