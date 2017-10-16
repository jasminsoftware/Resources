using Newtonsoft.Json;

namespace Jasmin.OrderSample
{
    /// <summary>
    /// Describes a line in an order.
    /// </summary>
    public class OrderLine
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        [JsonProperty("salesItem")]
        public string ItemId { get; set; }

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
        public Price Price { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [JsonProperty("itemType")]
        public int Type { get; set; }

        #endregion
    }
}
