using System.ComponentModel;

namespace Jasmin.IntegrationSample
{
    public enum SalesOrdersMenuOptions
    {
        [Description("Back To Main Menu")]
        Exit = 0,

        [Description("List First page of 20")]
        Sales_Orders_GetAll = 1,

        [Description("List filtered using Odata")]
        Sales_Orders_GetOdata = 2,

        [Description("Create Order")]
        Sales_Orders_Create = 3,

        [Description("Get LastCreated Order")]
        Sales_Orders_GetLastOrder = 4,

        [Description("Insert New Line on Order")]
        Sales_Orders_InsertLine = 5,

        [Description("Add Quantity (+2) on Order")]
        Sales_Orders_UpdateQtd = 6,

        [Description("Delete LastLine on Order")]
        Sales_Orders_DelLastLine = 7,

        [Description("Delete Last Order")]
        Sales_Orders_Del = 8,
    }
}
