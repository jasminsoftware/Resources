using System.Collections.Generic;

namespace Jasmin.IntegrationSample
{
    internal class BaseSalesItemResource
    {
        #region Members

        private ICollection<SalesItemPriceListLineResource> pricelistLines = new System.Collections.ObjectModel.Collection<SalesItemPriceListLineResource>();

        #endregion

        #region Public Properties

        public string ItemKey { get; set; }

        public string Description { get; set; }

        public string IncomeAccount { get; set; }
        
        public string ItemTaxSchema { get; set; }
        
        public string Unit { get; set; }

        public string BaseUnit { get; set; }

        public string ItemType { get; set; }

        public string Assortment { get; set; }

        public ICollection<SalesItemPriceListLineResource> PriceListLines { get { return this.pricelistLines; } set { this.pricelistLines = value; } }

        #endregion
    }
}
