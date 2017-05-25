using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jasmin.InvoiceSample
{
    /// <summary>
    /// The object with all the information of an order.
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [JsonProperty("id")]
        public Guid? InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        [JsonProperty("buyerCustomerParty")]
        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        [JsonProperty("company")]
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the discount.
        /// </summary>
        /// <value>
        /// The discount.
        /// </value>
        [JsonProperty("discount")]
        public double? Discount { get; set; }

        /// <summary>
        /// Gets or sets the order payable amount.
        /// </summary>
        /// <value>
        /// The order payable amount.
        /// </value>
        [JsonProperty("payableAmount")]
        public Money PayableAmount { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        /// <value>
        /// The order date.
        /// </value>
        [JsonProperty("documentDate")]
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        [JsonProperty("documentType")]
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the payment term.
        /// </summary>
        /// <value>
        /// The payment term.
        /// </value>
        [JsonProperty("paymentTerm")]
        public string PaymentTerm { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>
        /// The payment method.
        /// </value>
        [JsonProperty("paymentMethod")]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the delivery term.
        /// </summary>
        /// <value>
        /// The delivery term.
        /// </value>
        [JsonProperty("deliveryTerm")]
        public string DeliveryTerm { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        /// <value>
        /// The remarks.
        /// </value>
        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets the document lines.
        /// </summary>
        /// <value>
        /// The document lines.
        /// </value>
        [JsonProperty("documentLines")]
        public List<InvoiceLine> Lines { get; set; }
    }
}
