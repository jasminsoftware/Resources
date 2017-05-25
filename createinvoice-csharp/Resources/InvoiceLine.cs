using Newtonsoft.Json;

namespace Jasmin.InvoiceSample
{
    /// <summary>
    /// The object with the information of an invoice line.
    /// </summary>
    public class InvoiceLine
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        [JsonProperty("salesItem")]
        public string Item { get; set; }

        /// <summary>
        /// Gets or sets the item description.
        /// </summary>
        /// <value>
        /// The item description.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        [JsonProperty("quantity")]
        public double? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [JsonProperty("unitPrice")]
        public Money Price { get; set; }
    }
}
