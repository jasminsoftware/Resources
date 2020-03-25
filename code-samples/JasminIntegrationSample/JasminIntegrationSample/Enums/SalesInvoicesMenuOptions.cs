using System.ComponentModel;

namespace Jasmin.IntegrationSample
{
    public enum SalesInvoicesMenuOptions
    {
        [Description("Back To Main Menu")]
        Exit = 0,

        [Description("List First page of 20")]
        Sales_Invoices_GetAll = 1,

        [Description("List filtered using Odata")]
        Sales_Invoices_GetOdata = 2,

        [Description("Create Invoice")]
        Sales_Invoices_Create = 3,

        [Description("Get LastCreated Invoice")]
        Sales_Invoices_GetLastInvoice = 4,

        [Description("Delete Last Invoice")]
        Sales_Invoices_Del = 5,
    }
}
