using Newtonsoft.Json;

namespace CreateInvoice
{
    internal class InvoiceLineResource
    {
        #region Public Properties

        [JsonProperty("salesItem")]
        public string Item { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        public double? Quantity { get; set; }

        [JsonProperty("unitPrice")]
        public MoneyResource Price { get; set; }

        #endregion
    }
}
