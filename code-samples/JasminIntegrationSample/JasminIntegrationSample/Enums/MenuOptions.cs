using System.ComponentModel;

namespace Jasmin.IntegrationSample
{
    public enum MenuOptions
    {
        [Description("Exit")]
        Exit = 0,

        [Description("List Orders - First page of 20")]
        Sales_Orders_GetAll = 1,

        [Description("List Orders filtered using Odata")]
        Sales_Orders_GetOdata = 2,

        [Description("Create Sales Order")]
        Sales_Orders_Create = 3,

        [Description("Get LastCreated Sales Order")]
        Sales_Orders_GetLastOrder = 4,

        [Description("Insert New Line on Sales Order")]
        Sales_Orders_InsertLine = 5,

        [Description("Add Quantity (+2) on Sales Order")]
        Sales_Orders_UpdateQtd = 6,

        [Description("Delete LastLine on Sales Order")]
        Sales_Orders_DelLastLine = 7,

        [Description("Delete Last Sales Order")]
        Sales_Orders_Del = 8,
    }
}
