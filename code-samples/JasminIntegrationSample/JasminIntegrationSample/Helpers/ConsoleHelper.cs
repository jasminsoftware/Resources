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
            Console.WriteLine("This is your awesome menu.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_Orders_GetAll, MenuOptions.Sales_Orders_GetAll.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_Orders_GetOdata, MenuOptions.Sales_Orders_GetOdata.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_Orders_Create, MenuOptions.Sales_Orders_Create.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_Orders_GetLastOrder, MenuOptions.Sales_Orders_GetLastOrder.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_Orders_InsertLine, MenuOptions.Sales_Orders_InsertLine.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_Orders_UpdateQtd, MenuOptions.Sales_Orders_UpdateQtd.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_Orders_DelLastLine, MenuOptions.Sales_Orders_DelLastLine.ToName()));
            Console.WriteLine(string.Format("{0:D} - {1}", MenuOptions.Sales_Orders_Del, MenuOptions.Sales_Orders_Del.ToName()));
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

        private static void WriteWaitingMessage()
        {
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue . . .");
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
