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
            // Start the async task

            try
            {
                SessionContext session = ConsoleHelper.GetSessionArguments(args);
                if (session.IsValid())
                {
                    AuthenticationProvider authenticationProvider = new AuthenticationProvider(session.ClientId, session.ClientSecret);

                    SetSessionCompanyKeyAsync(session, authenticationProvider).Wait();

                    MenuOptions option = MenuOptions.Exit;
                    do
                    {
                        option = ConsoleHelper.GetMenuOption();
                        if (option != MenuOptions.Exit)
                        {
                            DoItAsync(session, authenticationProvider, option).Wait();
                        }
                    } while (option != MenuOptions.Exit);
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

        private static async Task<int> DoItAsync(SessionContext sessionContext, AuthenticationProvider authenticationProvider, MenuOptions option)
        {
            Console.WriteLine("");
            Console.WriteLine("selected option " + string.Format("{0:D} - {1}", option, option.ToName()));
            Console.WriteLine("");

            // Get the authorization access token
            OrdersController.AccountKey = sessionContext.AccountKey;
            OrdersController.SubscriptionKey = sessionContext.SubscriptionKey;
            OrdersController.CompanyKey = sessionContext.CompanyKey;
            OrdersController.CultureKey = sessionContext.CultureKey;
            OrdersController.AuthenticationProvider = authenticationProvider;

            switch (option)
            {
                case MenuOptions.Sales_Orders_GetAll:
                    await OrdersController.GetOrdersAsync();
                    break;

                case MenuOptions.Sales_Orders_GetOdata:
                    await OrdersController.GetTop5OrdersAsync();
                    break;

                case MenuOptions.Sales_Orders_GetLastOrder:
                    sessionContext.OrderResource = await OrdersController.GetLastOrderAsync(false);
                    break;

                case MenuOptions.Sales_Orders_Create:
                    await OrdersController.CreateOrderAsync();
                    /// Just to update
                    sessionContext.OrderResource = await OrdersController.GetLastOrderAsync(true);
                    break;

                case MenuOptions.Sales_Orders_InsertLine:
                    if (sessionContext.OrderResource == null)
                    {
                        sessionContext.OrderResource = await OrdersController.GetLastOrderAsync(true);
                    }

                    if (sessionContext.OrderResource != null)
                    { 
                        await OrdersController.InsertLineOnOrderAsync(sessionContext.OrderResource);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create an order first.");
                    }

                    break;

                case MenuOptions.Sales_Orders_UpdateQtd:
                    if (sessionContext.OrderResource == null)
                    {
                        sessionContext.OrderResource = await OrdersController.GetLastOrderAsync(true);
                    }

                    if (sessionContext.OrderResource != null)
                    {
                        await OrdersController.SetQuantityOnOrderLastLineAsync(sessionContext.OrderResource);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create an order first.");
                    }

                    break;

                case MenuOptions.Sales_Orders_DelLastLine:
                    if (sessionContext.OrderResource == null)
                    {
                        sessionContext.OrderResource = await OrdersController.GetLastOrderAsync(true);
                    }

                    if (sessionContext.OrderResource != null)
                    {
                        await OrdersController.DeleteLastLineOnOrderAsync(sessionContext.OrderResource);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create an order first.");
                    }

                    break;

                case MenuOptions.Sales_Orders_Del:
                    if (sessionContext.OrderResource == null)
                    {
                        sessionContext.OrderResource = await OrdersController.GetLastOrderAsync(true);
                    }

                    if (sessionContext.OrderResource != null)
                    {
                        await OrdersController.DeleteLastOrderAsync(sessionContext.OrderResource);
                    }
                    else
                    {
                        ConsoleHelper.WriteErrorLine("You must create an order first.");
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
            Console.WriteLine(string.IsNullOrEmpty(companyKey) ? "Company not found. Please create the company first." : String.Format("Company found: {0}.", companyKey));

            sessionContext.CompanyKey = companyKey;
        }

        #endregion
    }
}
