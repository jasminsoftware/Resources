using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    /// <summary>
    /// Describes an order.
    /// </summary>
    internal class SalesOrderResource
    {
        #region Members

        private ICollection<SalesOrderLineResource> documentLines = new System.Collections.ObjectModel.Collection<SalesOrderLineResource>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [JsonProperty("id")]
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the serie.
        /// </summary>
        [JsonProperty("serie")]
        public string Serie { get; set; }

        /// <summary>
        /// Gets or sets the serieNumber.
        /// </summary>
        [JsonProperty("seriesNumber")]
        public int SeriesNumber { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        [JsonProperty("buyerCustomerParty")]
        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets the discount.
        /// </summary>
        [JsonProperty("discount")]
        public double? Discount { get; set; }

        /// <summary>
        /// Gets or sets the order payable amount.
        /// </summary>
        [JsonProperty("payableAmount")]
        public MoneyResource PayableAmount { get; set; }

        /// <summary>
        /// Gets or sets the order payable amount.
        /// </summary>
        [JsonProperty("documentStatus")]
        public int DocumentStatus { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        [JsonProperty("documentDate")]
        public DateTime DocumentDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        [JsonProperty("documentType")]
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the payment term.
        /// </summary>
        [JsonProperty("paymentTerm")]
        public string PaymentTerm { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        [JsonProperty("paymentMethod")]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the payment method description.
        /// </summary>
        [JsonProperty("paymentMethodDescription")]
        public string PaymentMethodDescription { get; set; }

        /// <summary>
        /// Gets or sets the delivery term.
        /// </summary>
        [JsonProperty("deliveryTerm")]
        public string DeliveryTerm { get; set; }

        /// <summary>
        /// Gets or sets the sales channel.
        /// </summary>
        [JsonProperty("salesChannel")]
        public string SalesChannel { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        [JsonProperty("company")]
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        [JsonProperty("Currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the note to recipent.
        /// </summary>
        [JsonProperty("noteToRecipient")]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        [JsonProperty("remarks")]
        public string Remarks { get; set; }
        
        // <summary>
        /// The order's lines.
        /// </summary>
        [JsonProperty("documentLines")]
        public ICollection<SalesOrderLineResource> Lines { get { return this.documentLines; } set { this.documentLines = value; } }

        #endregion
    }
}
