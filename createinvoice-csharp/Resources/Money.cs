using Newtonsoft.Json;

namespace Jasmin.InvoiceSample

{
    /// <summary>
    /// The object with the information of a Money.
    /// </summary>
    public class Money
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("amount")]
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        [JsonProperty("symbol")]
        public string Currency { get; set; }
    }
}