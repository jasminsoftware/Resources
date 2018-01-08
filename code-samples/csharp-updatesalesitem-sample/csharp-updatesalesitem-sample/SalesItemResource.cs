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

        /*
                public string sqas
                {
                    get;
                    set;
                }

                public int SeriesNumber
                {
                    get;
                    set;
                }

                public string Company
                {
                    get;
                    set;
                }
                */
        #endregion
    }
}