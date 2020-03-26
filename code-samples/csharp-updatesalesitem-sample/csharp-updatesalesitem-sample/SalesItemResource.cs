using System;
using System.Collections.ObjectModel;

namespace UpdateSalesItem

{
    internal class SalesItemResource
    {
        #region Internal Properties

        public Guid Id
        {
            get;
            set;
        }

        public string Itemkey
        {
            get;
            set;
        }

        public Collection<PriceListLineResource> PriceListLines { get; set; }

        #endregion
    }
}