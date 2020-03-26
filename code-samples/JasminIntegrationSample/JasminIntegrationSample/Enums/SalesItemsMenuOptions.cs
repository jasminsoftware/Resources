using System.ComponentModel;

namespace Jasmin.IntegrationSample
{
    public enum SalesItemsMenuOptions
    {
        [Description("Back to Main Menu")]
        Exit = 0,

        [Description("Create Sales Item and Item (AllInOne)")]
        Sales_SalesItems_SalesItemAllInOne = 1,

        [Description("Create Item and then Sales Item Extension")]
        Sales_SalesItems_ItemAndSalesItem = 2,

        [Description("Get Last Sales Item Created")]
        Sales_SalesItems_GetLastSalesItem = 3,

        [Description("Set a Price")]
        Sales_SalesItems_SetPrices = 4,

        [Description("Update that Price")]
        Sales_SalesItems_UpdatePrice = 5,

        [Description("Delete Sales Item Extension")]
        Sales_SalesItems_DeleteSalesItem = 6,

        [Description("Add Sales Item Extension")]
        Sales_SalesItems_AddSalesItem = 7,

        [Description("Delete All Items Created on Sample")]
        Sales_SalesItems_DeleteAllItems = 8
    }
}
