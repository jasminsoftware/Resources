using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    internal class SalesItemPriceListLineResource
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
        public string SalesItem { get; set; }

        [JsonProperty("priceList")]
        public string PriceList { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("priceAmount")]
        public MoneyResource Price { get; set; }

        #endregion

    }
}
