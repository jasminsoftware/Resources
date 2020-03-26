using Newtonsoft.Json;

namespace UpdateSalesItem
{
    internal class MoneyResource
    {
        #region Public Properties

        [JsonProperty("amount")]
        public double Value { get; set; }

        [JsonProperty("symbol")]
        public string Currency { get; set; }

        #endregion
    }
}
