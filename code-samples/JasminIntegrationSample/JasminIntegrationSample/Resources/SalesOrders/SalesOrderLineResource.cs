using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    /// <summary>
    /// Describes a line in an order.
    /// </summary>
    internal class SalesOrderLineResource
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

        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        [JsonProperty("salesItem")]
        public string Item { get; set; }

        /// <summary>
        /// Gets or sets the item description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        [JsonProperty("quantity")]
        public double Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        [JsonProperty("unitPrice")]
        public MoneyResource Price { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [JsonProperty("itemType")]
        public int ItemType { get; set; }

        #endregion
    }
}
