using System;
using System.Linq;
using Jasmin.IntegrationSample.Extensions;

namespace Jasmin.IntegrationSample
{
    /// <summary>
    /// Helper Class to handle console UI.
    /// </summary>
    internal static class ConsoleHelper
    {
        #region Internal Methods

        internal static void WriteErrorLine(string message)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        internal static void WriteSuccessLine(string message)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        internal static void WriteWaitingMessage()
        {
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue . . .");
        }

        internal static SessionContext GetSessionArguments(string[] args)
        {
            SessionContext context = new SessionContext();

            if (args.Length == 1 && HelpRequired(args[0]))
            {
                DisplayHelp();
                return context;
            }
            else
            {
                Console.WriteLine("Starting ...");

                // Test if input arguments were supplied.

                // Get the Client Id

                string clientId = GetArgument(Constants.Session.ClientId, args);
                if (string.IsNullOrEmpty(clientId))
                {
                    return context;
                }

                context.ClientId = clientId;

                // Get the Client Secret

                string clientSecret = GetArgument(Constants.Session.ClientSecret, args);
                if (string.IsNullOrEmpty(clientId))
                {
                    return context;
                }

                context.ClientSecret = clientSecret;

                // Get the Account Key

                string accountKey = GetArgument(Constants.Session.AccountKey, args);
                if (string.IsNullOrEmpty(accountKey))
                {
                    return context;
                }

                context.AccountKey = accountKey;

                // Get the subscription key

                string subscriptionKey = GetArgument(Constants.Session.SubscriptionKey, args);
                if (string.IsNullOrEmpty(subscriptionKey))
                {
                    return context;
                }

                context.SubscriptionKey = subscriptionKey;

                return context;
            }
        }

        internal static MenuOptions GetMenuOption()
        {
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("This is your application menu.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("Please select your context");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_Customers, MenuOptions.Sales_Customers.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_SalesItems, MenuOptions.Sales_SalesItems.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_SalesOrders, MenuOptions.Sales_SalesOrders.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_SalesInvoices, MenuOptions.Sales_SalesInvoices.ToName()));
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Exit, MenuOptions.Exit.ToName()));
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("Tip: /h on the command line will show help on how to set arguments at start up.");
            Console.WriteLine("");
            Console.WriteLine("Type you option: ");

            ConsoleKeyInfo userOption = Console.ReadKey(true);

            MenuOptions option = MenuOptions.Exit;
            int maxOption = option.Max();
            int intOption = 0;

            if (!int.TryParse(userOption.KeyChar.ToString(), out intOption))
            {
                Console.WriteLine("Not a valid Input. Exiting application.");
                option = MenuOptions.Exit;
            }
            else
            {
                if (intOption == 0)
                {
                    Console.WriteLine("Exiting application. Bye!");
                }
                else if (intOption < 0 || intOption > maxOption)
                {
                    Console.WriteLine("Not a valid Input. Exiting application.");
                    option = MenuOptions.Exit;
                }
                else
                {
                    option = (MenuOptions)intOption;
                }
            }

            if (intOption == 0)
            {
               WriteWaitingMessage();
                Console.ReadKey();
            }

            return option;
        }

        internal static CustomersMenuOptions GetCustomersMenuOption()
        {
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("This is your Customers menu.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", CustomersMenuOptions.Sales_Customers_CustomerAllInOne, CustomersMenuOptions.Sales_Customers_CustomerAllInOne.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", CustomersMenuOptions.Sales_Customers_PartyAndCustomer, CustomersMenuOptions.Sales_Customers_PartyAndCustomer.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", CustomersMenuOptions.Sales_Customers_GetLastCustomer, CustomersMenuOptions.Sales_Customers_GetLastCustomer.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", CustomersMenuOptions.Sales_Customers_UpdateCityAddress, CustomersMenuOptions.Sales_Customers_UpdateCityAddress.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", CustomersMenuOptions.Sales_Customers_UpdatePaymentTerm, CustomersMenuOptions.Sales_Customers_UpdatePaymentTerm.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", CustomersMenuOptions.Sales_Customers_DeleteCustomer, CustomersMenuOptions.Sales_Customers_DeleteCustomer.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", CustomersMenuOptions.Sales_Customers_AddCustomer, CustomersMenuOptions.Sales_Customers_AddCustomer.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", CustomersMenuOptions.Sales_Customers_DeleteAllParties, CustomersMenuOptions.Sales_Customers_DeleteAllParties.ToName()));
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", CustomersMenuOptions.Exit, CustomersMenuOptions.Exit.ToName()));
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine("Type you option: ");

            ConsoleKeyInfo userOption = Console.ReadKey(true);

            CustomersMenuOptions option = CustomersMenuOptions.Exit;
            int maxOption = option.Max();
            int intOption = 0;

            if (!int.TryParse(userOption.KeyChar.ToString(), out intOption))
            {
                Console.WriteLine("");
                Console.WriteLine("Not a valid Input. Back to Main Menu.");
                intOption = 0;
            }
            else
            {
                if (intOption == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("selected option " + string.Format("{0:D} - {1}", option, option.ToName()));
                }
                else if (intOption < 0 || intOption > maxOption)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Not a valid Input. Back to Main Menu.");
                    intOption = 0;
                }
                else
                {
                    option = (CustomersMenuOptions)intOption;
                }
            }

            return option;
        }

        internal static SalesItemsMenuOptions GetSalesItemsMenuOption()
        {
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("This is your Sales Items menu.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", SalesItemsMenuOptions.Sales_SalesItems_SalesItemAllInOne, SalesItemsMenuOptions.Sales_SalesItems_SalesItemAllInOne.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesItemsMenuOptions.Sales_SalesItems_ItemAndSalesItem, SalesItemsMenuOptions.Sales_SalesItems_ItemAndSalesItem.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesItemsMenuOptions.Sales_SalesItems_GetLastSalesItem, SalesItemsMenuOptions.Sales_SalesItems_GetLastSalesItem.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesItemsMenuOptions.Sales_SalesItems_SetPrices, SalesItemsMenuOptions.Sales_SalesItems_SetPrices.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesItemsMenuOptions.Sales_SalesItems_UpdatePrice, SalesItemsMenuOptions.Sales_SalesItems_UpdatePrice.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesItemsMenuOptions.Sales_SalesItems_DeleteSalesItem, SalesItemsMenuOptions.Sales_SalesItems_DeleteSalesItem.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesItemsMenuOptions.Sales_SalesItems_AddSalesItem, SalesItemsMenuOptions.Sales_SalesItems_AddSalesItem.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesItemsMenuOptions.Sales_SalesItems_DeleteAllItems, SalesItemsMenuOptions.Sales_SalesItems_DeleteAllItems.ToName()));
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", SalesItemsMenuOptions.Exit, SalesItemsMenuOptions.Exit.ToName()));
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine("Type you option: ");

            ConsoleKeyInfo userOption = Console.ReadKey(true);

            SalesItemsMenuOptions option = SalesItemsMenuOptions.Exit;
            int maxOption = option.Max();
            int intOption = 0;

            if (!int.TryParse(userOption.KeyChar.ToString(), out intOption))
            {
                Console.WriteLine("");
                Console.WriteLine("Not a valid Input. Back to Main Menu.");
                intOption = 0;
            }
            else
            {
                if (intOption == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("selected option " + string.Format("{0:D} - {1}", option, option.ToName()));
                }
                else if (intOption < 0 || intOption > maxOption)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Not a valid Input. Back to Main Menu.");
                    intOption = 0;
                }
                else
                {
                    option = (SalesItemsMenuOptions)intOption;
                }
            }

            return option;
        }

        internal static SalesOrdersMenuOptions GetSalesOrdersMenuOption()
        {
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("This is your Sales Orders menu.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", SalesOrdersMenuOptions.Sales_Orders_GetAll, SalesOrdersMenuOptions.Sales_Orders_GetAll.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesOrdersMenuOptions.Sales_Orders_GetOdata, SalesOrdersMenuOptions.Sales_Orders_GetOdata.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesOrdersMenuOptions.Sales_Orders_Create, SalesOrdersMenuOptions.Sales_Orders_Create.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesOrdersMenuOptions.Sales_Orders_GetLastOrder, SalesOrdersMenuOptions.Sales_Orders_GetLastOrder.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesOrdersMenuOptions.Sales_Orders_InsertLine, SalesOrdersMenuOptions.Sales_Orders_InsertLine.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesOrdersMenuOptions.Sales_Orders_UpdateQtd, SalesOrdersMenuOptions.Sales_Orders_UpdateQtd.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesOrdersMenuOptions.Sales_Orders_DelLastLine, SalesOrdersMenuOptions.Sales_Orders_DelLastLine.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesOrdersMenuOptions.Sales_Orders_Del, SalesOrdersMenuOptions.Sales_Orders_Del.ToName()));
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", SalesOrdersMenuOptions.Exit, SalesOrdersMenuOptions.Exit.ToName()));
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine("Type you option: ");

            ConsoleKeyInfo userOption = Console.ReadKey(true);

            SalesOrdersMenuOptions option = SalesOrdersMenuOptions.Exit;
            int maxOption = option.Max();
            int intOption = 0;

            if (!int.TryParse(userOption.KeyChar.ToString(), out intOption))
            {
                Console.WriteLine("");
                Console.WriteLine("Not a valid Input. Back to Main Menu.");
                intOption = 0;
            }
            else
            {
                if (intOption == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("selected option " + string.Format("{0:D} - {1}", option, option.ToName()));
                }
                else if (intOption < 0 || intOption > maxOption)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Not a valid Input. Back to Main Menu.");
                    intOption = 0;
                }
                else
                {
                    option = (SalesOrdersMenuOptions)intOption;
                }
            }

            return option;
        }

        internal static SalesInvoicesMenuOptions GetSalesInvoicesMenuOption()
        {
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("This is your Sales Invoices menu.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", SalesInvoicesMenuOptions.Sales_Invoices_GetAll, SalesInvoicesMenuOptions.Sales_Invoices_GetAll.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesInvoicesMenuOptions.Sales_Invoices_GetOdata, SalesInvoicesMenuOptions.Sales_Invoices_GetOdata.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesInvoicesMenuOptions.Sales_Invoices_Create, SalesInvoicesMenuOptions.Sales_Invoices_Create.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesInvoicesMenuOptions.Sales_Invoices_GetLastInvoice, SalesInvoicesMenuOptions.Sales_Invoices_GetLastInvoice.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", SalesInvoicesMenuOptions.Sales_Invoices_Del, SalesInvoicesMenuOptions.Sales_Invoices_Del.ToName()));
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", SalesInvoicesMenuOptions.Exit, SalesInvoicesMenuOptions.Exit.ToName()));
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine("Type you option: ");

            ConsoleKeyInfo userOption = Console.ReadKey(true);

            SalesInvoicesMenuOptions option = SalesInvoicesMenuOptions.Exit;
            int maxOption = option.Max();
            int intOption = 0;

            if (!int.TryParse(userOption.KeyChar.ToString(), out intOption))
            {
                Console.WriteLine("");
                Console.WriteLine("Not a valid Input. Back to Main Menu.");
                intOption = 0;
            }
            else
            {
                if (intOption == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("selected option " + string.Format("{0:D} - {1}", option, option.ToName()));
                }
                else if (intOption < 0 || intOption > maxOption)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Not a valid Input. Back to Main Menu.");
                    intOption = 0;
                }
                else
                {
                    option = (SalesInvoicesMenuOptions)intOption;
                }
            }

            return option;
        }

        #endregion

        #region Private Methods

        private static bool HelpRequired(string param)
        {
            return param == "-h" || param == "--help" || param == "/?" || param == "/h";
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("This Console App provides a sample on how to authenticate and communicate with Jasmin.");
            Console.WriteLine("");
            Console.WriteLine("Jasmin.IntegrationSample [/ClientId:yourclienteid] [/ClientSecret:yourclientsecret] [/AccountKey:youAccount] [/SubscriptionKey:yourSubscription]");
            Console.WriteLine("");
            Console.WriteLine("/ClientId         Your app's client id ");
            Console.WriteLine("                  OPTIONAL: The program will request for it's input if argument is not set.");
            Console.WriteLine("");
            Console.WriteLine("/ClientSecret     Your app's client secret");
            Console.WriteLine("                  OPTIONAL: The program will request for it's input if argument is not set.");
            Console.WriteLine("");
            Console.WriteLine("/AccountKey       The AccountKey of your subscription");
            Console.WriteLine("                  OPTIONAL: The program will request for it's input if argument is not set.");
            Console.WriteLine("");
            Console.WriteLine("/SubscriptionKey  The SubscriptionKey of your subscription");
            Console.WriteLine("                  OPTIONAL: The program will request for it's input if argument is not set.");
            Console.WriteLine("");
            Console.WriteLine("After authentication validation a menu will be displayed with available options.");
            WriteWaitingMessage();
            Console.ReadKey();
        }

        private static string GetArgument(string arg, string[] args)
        {
            // Test if input arguments were supplied.
            string argument = string.Empty;
            string argumentName = arg.Substring(1, arg.Length - 2);
            if (args.Length > 0)
            {
                if (args.Any(a => a.StartsWith(arg)))
                {
                    argument = args.FirstOrDefault(a => a.StartsWith(arg)).Substring(arg.Length);
                }
            }

            while (string.IsNullOrEmpty(argument))
            {
                Console.WriteLine("");
                Console.WriteLine(String.Format("Type your {0}: ", argumentName));
                argument = Console.ReadLine();
                if (string.IsNullOrEmpty(argument))
                {
                    WriteErrorLine(String.Format("ERROR: {0} cannot be empty.", argumentName));
                }
            }

            return argument;
        }

        #endregion
    }
}
