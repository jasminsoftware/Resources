using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    internal class SalesItemResource
    {
        #region Members

        private ICollection<SalesItemPriceListLineResource> pricelistLines = new System.Collections.ObjectModel.Collection<SalesItemPriceListLineResource>();

        #endregion

        #region Public Properties

        public string BaseEntityKey { get; set; }

        public string ItemKey { get; set; }
        
        public string IncomeAccount { get; set; }
        
        public string ItemTaxSchema { get; set; }
        
        public string Unit { get; set; }

        public ICollection<SalesItemPriceListLineResource> PriceListLines { get { return this.pricelistLines; } set { this.pricelistLines = value; } }

        #endregion
    }
}
