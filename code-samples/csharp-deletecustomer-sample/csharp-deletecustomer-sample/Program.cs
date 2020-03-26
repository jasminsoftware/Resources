using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DeleteCustomer
{
    public static class Program
    {
        #region Constants

        private const string ApiBaseAddress = "https://my.jasminsoftware.com/api";

        private const string AccountKey = "XXXXX"; // TODO: put your account key here
        private const string SubscriptionKey = "XXXXX-XXXX"; // TODO: put your account key here

        private const string AccessToken = "XXXXXXXXXXX"; // TODO: put the authorization access token here (this should be obtained previously)

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            // Start

            Console.WriteLine("Jasmin rocks! Let's delete a customer using the Web API...");

            // Handle errors

            try
            {
                // Create the HTTP client to send the request

                using (HttpClient client = new HttpClient())
                {
                    // Set the authorization header

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                    // Select the customer that will be deleted

                    Console.Write("Insert Customer Key that will be deleted: ");
                    string partyKey = Console.ReadLine();

                    // URI of the endpoint

                    string requestUri = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}/salesCore/customerParties/{3}", ApiBaseAddress, AccountKey, SubscriptionKey, partyKey);

                    // Send the request (DEL)

                    Console.WriteLine("Kicking the CustomerParties endpoint to delete the customer...");

                    Uri uri = new Uri(requestUri);

                    using (HttpResponseMessage response = await client.DeleteAsync(uri))
                    {
                        // Failed?

                        if (!response.IsSuccessStatusCode)
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "The requested failed with status code {0} ({1}).", (int)response.StatusCode, response.StatusCode));

                            if (!string.IsNullOrWhiteSpace(errorContent))
                            {
                                sb.Append(string.Format(CultureInfo.CurrentCulture, "Message: {0}.", errorContent));
                            }

                            throw new InvalidOperationException(sb.ToString());
                        }

                        // Succeeded

                        string customerId = await response.Content.ReadAsStringAsync();

                        Console.WriteLine("The customer was deleted with success.");

                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
            }

            // End

            Console.Write("Press any key to end... ");
            Console.ReadKey();
        }

        #endregion
    }
}
