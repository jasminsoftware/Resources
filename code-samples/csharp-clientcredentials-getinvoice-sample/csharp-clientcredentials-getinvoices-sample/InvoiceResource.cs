using System;

namespace GetInvoices
{
    internal class InvoiceResource
    {
        #region Public Properties

        public Guid Id
        {
            get;
            set;
        }

        public string DocumentType
        {
            get;
            set;
        }

        public string Serie
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

        #endregion
    }
}
