using System.ComponentModel;

namespace Jasmin.IntegrationSample
{
    public enum CustomersMenuOptions
    {
        [Description("Back to Main Menu")]
        Exit = 0,

        [Description("Create Customer and Party (AllInOne)")]
        Sales_Customers_CustomerAllInOne = 1,

        [Description("Create Party and then Customer Extension")]
        Sales_Customers_PartyAndCustomer = 2,

        [Description("Get Last Created Customer")]
        Sales_Customers_GetLastCustomer = 3,

        [Description("Update Last Customer City")]
        Sales_Customers_UpdateCityAddress = 4,

        [Description("Update Last Customer PaymentTerm")]
        Sales_Customers_UpdatePaymentTerm = 5,

        [Description("Delete Last Customer Extension")]
        Sales_Customers_DeleteCustomer = 6,

        [Description("Add Customer Extension")]
        Sales_Customers_AddCustomer = 7,

        [Description("Delete All Parties Created on Sample")]
        Sales_Customers_DeleteAllParties = 8
    }
}
