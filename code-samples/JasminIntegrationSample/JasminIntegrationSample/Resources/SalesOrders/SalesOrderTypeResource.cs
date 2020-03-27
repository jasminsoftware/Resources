using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    internal class SalesOrderTypeResource
    {
        #region Members

        private ICollection<SalesOrderTypeLineSerieResource> documentLines = new System.Collections.ObjectModel.Collection<SalesOrderTypeLineSerieResource>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        [JsonProperty("company")]
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the typeKey.
        /// </summary>
        [JsonProperty("typeKey")]
        public string TypeKey { get; set; }

        /// <summary>
        /// Gets or sets the order nature.
        /// </summary>
        [JsonProperty("orderNature")]
        public string OrderNature { get; set; }

        // <summary>
        /// The order's lines.
        /// </summary>
        [JsonProperty("documentTypeSeries")]
        public ICollection<SalesOrderTypeLineSerieResource> Lines { get { return this.documentLines; } set { this.documentLines = value; } }

        #endregion
    }
}
