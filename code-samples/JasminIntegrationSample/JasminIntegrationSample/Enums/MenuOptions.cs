using System.ComponentModel;

namespace Jasmin.IntegrationSample
{
    public enum MenuOptions
    {
        [Description("Exit")]
        Exit = 0,

        [Description("Customers")]
        Sales_Customers = 1,

        [Description("Sales Items")]
        Sales_SalesItems = 2,

        [Description("Sales Orders")]
        Sales_SalesOrders = 3,

        [Description("Sales Invoices")]
        Sales_SalesInvoices = 4
    }
}
