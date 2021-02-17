using System;
using System.Threading.Tasks;
using Jasmin.IntegrationSample.Extensions;

namespace Jasmin.IntegrationSample
{
    /// <summary>
    /// The static class that holds the application starting point.
    /// </summary>
    public static class Program
    {
        #region Public Methods

        /// <summary>
        /// The starting point of the application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            try
            {
                // Handle requires arguments

                SessionContext session = ConsoleHelper.GetSessionArguments(args);
                if (session.IsValid())
                {
                    AuthenticationProvider authenticationProvider = new AuthenticationProvider(session.ClientId, session.ClientSecret);

                    // For this sample purpose we will select the company to use.

                    SetSessionCompanyKeyAsync(session, authenticationProvider).Wait();

                    if (!string.IsNullOrEmpty(session.CompanyKey))
                    {
                        // For this sample purpose we will allow negative stock on this company.

                        SetAllowNegativeStockAsync(session, authenticationProvider).Wait();

                        // Display Main Menu

                        MenuOptions option = MenuOptions.Exit;
                        do
                        {
                            option = ConsoleHelper.GetMenuOption();
                            if (option != MenuOptions.Exit)
                            {
                                HandleMainMenuOptionAsync(session, authenticationProvider, option).Wait();
                            }
                        } while (option != MenuOptions.Exit);
                    }
                    else
                    {
                        ConsoleHelper.WriteWaitingMessage();
                        Console.ReadKey();
                    }
                }
            }
            catch(Exception ex)
            {
                ConsoleHelper.WriteErrorLine(ex.Message);
                Console.ReadKey();
            }
        }

        #endregion

        #region Private Methods

        private static async Task<int> HandleMainMenuOptionAsync(SessionContext sessionContext, AuthenticationProvider authenticationProvider, MenuOptions option)
        {
            Console.WriteLine("");
            Console.WriteLine("selected option " + string.Format("{0:D} - {1}", option, option.ToName()));
            Console.WriteLine("");

            // Display Sub Menu according to Menu Selection.

            switch (option)
            {
                case MenuOptions.Sales_Customers:
                    CustomersMenuOptions customersOption = CustomersMenuOptions.Exit;
                    do
                    {
                        customersOption = ConsoleHelper.GetCustomersMenuOption();
                        if (customersOption != CustomersMenuOptions.Exit)
                        {
                            await HandleCustomersOperationAsync(sessionContext, authenticationProvider, customersOption);
                        }
                    } while (customersOption != CustomersMenuOptions.Exit);
                    break;

                case MenuOptions.Sales_SalesItems:

                    SalesItemsMenuOptions salesItemsOption = SalesItemsMenuOptions.Exit;
                    do
                    {
                        salesItemsOption = ConsoleHelper.GetSalesItemsMenuOption();
                        if (salesItemsOption != SalesItemsMenuOptions.Exit)
                        {
                            await HandleSalesItemsOperationAsync(sessionContext, authenticationProvider, salesItemsOption);
                        }
                    } while (salesItemsOption != SalesItemsMenuOptions.Exit);
                    break;

                case MenuOptions.Sales_SalesOrders:
                    SalesOrdersMenuOptions salesOrdersOption = SalesOrdersMenuOptions.Exit;
                    do
                    {
                        salesOrdersOption = ConsoleHelper.GetSalesOrdersMenuOption();
                        if (salesOrdersOption != SalesOrdersMenuOptions.Exit)
                        {
                            await HandleSalesOrdersOperationAsync(sessionContext, authenticationProvider, salesOrdersOption);
                        }
                    } while (salesOrdersOption != SalesOrdersMenuOptions.Exit);

                    break;
                case MenuOptions.Sales_SalesInvoices:
                    SalesInvoicesMenuOptions salesInvoicesOption = SalesInvoicesMenuOptions.Exit;
                    do
                    {
                        salesInvoicesOption = ConsoleHelper.GetSalesInvoicesMenuOption();
                        if (salesInvoicesOption != SalesInvoicesMenuOptions.Exit)
                        {
                            await HandleSalesInvoicesOperationAsync(sessionContext, authenticationProvider, salesInvoicesOption);
                        }
                    } while (salesInvoicesOption != SalesInvoicesMenuOptions.Exit);

                    break;
            }

            return 0;
        }

        private static async Task<int> HandleCustomersOperationAsync(SessionContext sessionContext, AuthenticationProvider authenticationProvider, CustomersMenuOptions option)
        {
            Console.WriteLine("");
            Console.WriteLine("selected option " + string.Format("{0:D} - {1}", option, option.ToName()));
            Console.WriteLine("");

            // Get the authorization access token

            CustomersController.AccountKey = sessionContext.AccountKey;
            CustomersController.SubscriptionKey = sessionContext.SubscriptionKey;
            CustomersController.CultureKey = sessionContext.CultureKey;
            CustomersController.AuthenticationProvider = authenticationProvider;

            switch (option)
            {
                case CustomersMenuOptions.Sales_Customers_CustomerAllInOne:
                    sessionContext.PartyKey = await CustomersController.CreateCustomerAndPartyAsync();
                    break;

                case CustomersMenuOptions.Sales_Customers_PartyAndCustomer:
                    sessionContext.PartyKey = await CustomersController.CreatePartyAndCustomerExtensionAsync();
                    break;

                case CustomersMenuOptions.Sales_Customers_GetLastCustomer:
                    sessionContext.PartyKey = await CustomersController.GetLastCustomerAsync(false);
                    break;

                case CustomersMenuOptions.Sales_Customers_UpdateCityAddress:
                    if (sessionContext.PartyKey == null)
                    {
                        sessionContext.PartyKey = await CustomersController.GetLastCustomerAsync(true);
                    }

                    if (sessionContext.PartyKey != null)
                    {
                        await CustomersController.UpdateCustomerCityAddressAsync(sessionContext.PartyKey);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create a customer first.");
                    }

                    break;

                case CustomersMenuOptions.Sales_Customers_UpdatePaymentTerm:
                    if (sessionContext.PartyKey == null)
                    {
                        sessionContext.PartyKey = await CustomersController.GetLastCustomerAsync(true);
                    }

                    if (sessionContext.PartyKey != null)
                    {
                        await CustomersController.UpdateCustomerPaymentTermAsync(sessionContext.PartyKey);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create a customer first.");
                    }

                    break;

                case CustomersMenuOptions.Sales_Customers_DeleteCustomer:
                    if (sessionContext.PartyKey == null)
                    {
                        sessionContext.PartyKey = await CustomersController.GetLastCustomerAsync(true);
                    }

                    if (sessionContext.PartyKey != null)
                    {
                        await CustomersController.DeleteCustomerExtensionAsync(sessionContext.PartyKey);
                        sessionContext.DeletedCustomerKey = sessionContext.PartyKey;
                        sessionContext.PartyKey = null;
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create a customer first.");
                    }

                    break;

                case CustomersMenuOptions.Sales_Customers_AddCustomer:
                    if (sessionContext.DeletedCustomerKey != null)
                    {
                        await CustomersController.CreateCustomerExtensionAsync(sessionContext.DeletedCustomerKey);
                        sessionContext.PartyKey = sessionContext.DeletedCustomerKey;
                        sessionContext.DeletedCustomerKey = null;
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must delete a customer first.");
                    }

                    break;
                case CustomersMenuOptions.Sales_Customers_DeleteAllParties:
                    await CustomersController.DeleteAllPartiesCreatedOnSample();
                    break;
            }

            return 0;
        }

        private static async Task<int> HandleSalesItemsOperationAsync(SessionContext sessionContext, AuthenticationProvider authenticationProvider, SalesItemsMenuOptions option)
        {
            Console.WriteLine("");
            Console.WriteLine("selected option " + string.Format("{0:D} - {1}", option, option.ToName()));
            Console.WriteLine("");

            // Get the authorization access token

            SalesItemsController.AccountKey = sessionContext.AccountKey;
            SalesItemsController.SubscriptionKey = sessionContext.SubscriptionKey;
            SalesItemsController.CultureKey = sessionContext.CultureKey;
            SalesItemsController.AuthenticationProvider = authenticationProvider;

            switch (option)
            {
                case SalesItemsMenuOptions.Sales_SalesItems_SalesItemAllInOne:
                    sessionContext.ItemKey = await SalesItemsController.CreateSalesItemAndItemAsync();
                    break;

                case SalesItemsMenuOptions.Sales_SalesItems_ItemAndSalesItem:
                    sessionContext.ItemKey = await SalesItemsController.CreateItemAndSalesItemExtensionAsync();
                    break;

                case SalesItemsMenuOptions.Sales_SalesItems_GetLastSalesItem:
                    sessionContext.ItemKey = await SalesItemsController.GetLastCreatedSalesItemAsync(false);
                    break;

                case SalesItemsMenuOptions.Sales_SalesItems_SetPrices:
                    if (sessionContext.ItemKey == null)
                    {
                        sessionContext.ItemKey = await SalesItemsController.GetLastCreatedSalesItemAsync(true);
                    }

                    if (sessionContext.ItemKey != null)
                    {
                        await SalesItemsController.InsertPriceOnSalesItemAsync(sessionContext.ItemKey);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create a salesItem first.");
                    }

                    break;

                case SalesItemsMenuOptions.Sales_SalesItems_UpdatePrice:
                    if (sessionContext.ItemKey == null)
                    {
                        sessionContext.ItemKey = await SalesItemsController.GetLastCreatedSalesItemAsync(true);
                    }

                    if (sessionContext.ItemKey != null)
                    {
                        await SalesItemsController.SetNewPriceAsync(sessionContext.ItemKey);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create a salesItem first.");
                    }
                    break;

                case SalesItemsMenuOptions.Sales_SalesItems_DeleteSalesItem:
                    if (sessionContext.ItemKey == null)
                    {
                        sessionContext.ItemKey = await SalesItemsController.GetLastCreatedSalesItemAsync(true);
                    }

                    if (sessionContext.ItemKey != null)
                    {
                        await SalesItemsController.DeleteSalesItemExtensionAsync(sessionContext.ItemKey);
                        sessionContext.DeletedItemKey = sessionContext.ItemKey;
                        sessionContext.ItemKey = null;
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create a SalesItem first.");
                    }

                    break;

                case SalesItemsMenuOptions.Sales_SalesItems_AddSalesItem:
                    if (sessionContext.DeletedItemKey != null)
                    {
                        await SalesItemsController.CreateSalesItemExtensionAsync(sessionContext.DeletedItemKey);
                        sessionContext.ItemKey = sessionContext.DeletedItemKey;
                        sessionContext.DeletedItemKey = null;
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must delete a sales item first.");
                    }

                    break;
                case SalesItemsMenuOptions.Sales_SalesItems_DeleteAllItems:
                    await SalesItemsController.DeleteAllItemsCreatedOnSample();
                    break;
            }

            return 0;
        }

        private static async Task<int> HandleSalesOrdersOperationAsync(SessionContext sessionContext, AuthenticationProvider authenticationProvider, SalesOrdersMenuOptions option)
        {
            Console.WriteLine("");
            Console.WriteLine("selected option " + string.Format("{0:D} - {1}", option, option.ToName()));
            Console.WriteLine("");

            // Get the authorization access token

            SalesOrdersController.AccountKey = sessionContext.AccountKey;
            SalesOrdersController.SubscriptionKey = sessionContext.SubscriptionKey;
            SalesOrdersController.CompanyKey = sessionContext.CompanyKey;
            SalesOrdersController.CultureKey = sessionContext.CultureKey;
            SalesOrdersController.AuthenticationProvider = authenticationProvider;

            switch (option)
            {
                case SalesOrdersMenuOptions.Sales_Orders_GetAll:
                    await SalesOrdersController.GetOrdersAsync();
                    break;

                case SalesOrdersMenuOptions.Sales_Orders_GetOdata:
                    await SalesOrdersController.GetTop5OrdersAsync();
                    break;

                case SalesOrdersMenuOptions.Sales_Orders_GetLastOrder:
                    sessionContext.OrderResource = await SalesOrdersController.GetLastOrderAsync(false);
                    break;

                case SalesOrdersMenuOptions.Sales_Orders_Create:
                    await SalesOrdersController.CreateOrderAsync();
                    /// Just to update
                    sessionContext.OrderResource = await SalesOrdersController.GetLastOrderAsync(true);
                    break;

                case SalesOrdersMenuOptions.Sales_Orders_InsertLine:
                    if (sessionContext.OrderResource == null)
                    {
                        sessionContext.OrderResource = await SalesOrdersController.GetLastOrderAsync(true);
                    }

                    if (sessionContext.OrderResource != null)
                    {
                        await SalesOrdersController.InsertLineOnOrderAsync(sessionContext.OrderResource);
                        sessionContext.OrderResource = await SalesOrdersController.GetLastOrderAsync(true);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create an order first.");
                    }

                    break;

                case SalesOrdersMenuOptions.Sales_Orders_UpdateQtd:
                    if (sessionContext.OrderResource == null)
                    {
                        sessionContext.OrderResource = await SalesOrdersController.GetLastOrderAsync(true);
                    }

                    if (sessionContext.OrderResource != null)
                    {
                        await SalesOrdersController.SetQuantityOnOrderLastLineAsync(sessionContext.OrderResource);
                        sessionContext.OrderResource = await SalesOrdersController.GetLastOrderAsync(true);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create an order first.");
                    }

                    break;

                case SalesOrdersMenuOptions.Sales_Orders_DelLastLine:
                    if (sessionContext.OrderResource == null)
                    {
                        sessionContext.OrderResource = await SalesOrdersController.GetLastOrderAsync(true);
                    }

                    if (sessionContext.OrderResource != null)
                    {
                        await SalesOrdersController.DeleteLastLineOnOrderAsync(sessionContext.OrderResource);
                        sessionContext.OrderResource = await SalesOrdersController.GetLastOrderAsync(true);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create an order first.");
                    }

                    break;

                case SalesOrdersMenuOptions.Sales_Orders_Del:
                    if (sessionContext.OrderResource == null)
                    {
                        sessionContext.OrderResource = await SalesOrdersController.GetLastOrderAsync(true);
                    }

                    if (sessionContext.OrderResource != null)
                    {
                        await SalesOrdersController.DeleteOrderAsync(sessionContext.OrderResource);
                        sessionContext.OrderResource = null;
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create an order first.");
                    }

                    break;
            }

            return 0;
        }

        private static async Task<int> HandleSalesInvoicesOperationAsync(SessionContext sessionContext, AuthenticationProvider authenticationProvider, SalesInvoicesMenuOptions option)
        {
            Console.WriteLine("");
            Console.WriteLine("selected option " + string.Format("{0:D} - {1}", option, option.ToName()));
            Console.WriteLine("");

            // Get the authorization access token

            SalesInvoicesController.AccountKey = sessionContext.AccountKey;
            SalesInvoicesController.SubscriptionKey = sessionContext.SubscriptionKey;
            SalesInvoicesController.CompanyKey = sessionContext.CompanyKey;
            SalesInvoicesController.CultureKey = sessionContext.CultureKey;
            SalesInvoicesController.AuthenticationProvider = authenticationProvider;

            switch (option)
            {
                case SalesInvoicesMenuOptions.Sales_Invoices_GetAll:
                    await SalesInvoicesController.GetInvoicesAsync();
                    break;

                case SalesInvoicesMenuOptions.Sales_Invoices_GetOdata:
                    await SalesInvoicesController.GetTop5InvoicesAsync();
                    break;

                case SalesInvoicesMenuOptions.Sales_Invoices_GetLastInvoice:
                    sessionContext.InvoiceResource = await SalesInvoicesController.GetLastInvoiceAsync(false);
                    break;

                case SalesInvoicesMenuOptions.Sales_Invoices_Create:
                    await SalesInvoicesController.CreateInvoiceAsync();
                    /// Just to update
                    sessionContext.InvoiceResource = await SalesInvoicesController.GetLastInvoiceAsync(true);
                    break;


                case SalesInvoicesMenuOptions.Sales_Invoices_Del:
                    if (sessionContext.InvoiceResource == null)
                    {
                        sessionContext.InvoiceResource = await SalesInvoicesController.GetLastInvoiceAsync(true);
                    }

                    if (sessionContext.InvoiceResource != null)
                    {
                        await SalesInvoicesController.DeleteInvoiceAsync(sessionContext.InvoiceResource);
                        sessionContext.InvoiceResource = null;
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create an invoice first.");
                    }

                    break;
            }

            return 0;
        }

        private static async Task SetSessionCompanyKeyAsync(SessionContext sessionContext, AuthenticationProvider authenticationProvider)
        {
            string companyKey = string.Empty;

            CompaniesController.AccountKey = sessionContext.AccountKey;
            CompaniesController.SubscriptionKey = sessionContext.SubscriptionKey;
            CompaniesController.CultureKey = sessionContext.CultureKey;
            CompaniesController.AuthenticationProvider = authenticationProvider;

            companyKey = await CompaniesController.GetCompanyKeyAsync();
            Console.WriteLine("");
            if (string.IsNullOrEmpty(companyKey))
            {
                ConsoleHelper.WriteErrorLine("Company not found. Please access to Jasmin and create a company first.");
                Console.WriteLine("Exiting application.");
            }
            else
            {
                Console.WriteLine(String.Format("Company found: {0}.", companyKey));
            }

            sessionContext.CompanyKey = companyKey;
        }


        private static async Task SetAllowNegativeStockAsync(SessionContext sessionContext, AuthenticationProvider authenticationProvider)
        {
            if (string.IsNullOrEmpty(sessionContext.CompanyKey))
            {
                Console.WriteLine("");
                Console.WriteLine("No company found. Unable to Allow Negative Stock.");

                return;
            }

            LogisticsSetupsController.AccountKey = sessionContext.AccountKey;
            LogisticsSetupsController.SubscriptionKey = sessionContext.SubscriptionKey;
            LogisticsSetupsController.CultureKey = sessionContext.CultureKey;
            LogisticsSetupsController.AuthenticationProvider = authenticationProvider;

            await LogisticsSetupsController.AllowNegativeStockOnCompanyAsync(sessionContext.CompanyKey);
        }

        #endregion
    }
}
