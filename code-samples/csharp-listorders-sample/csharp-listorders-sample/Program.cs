using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ListOrders
{
    public static class Program
    {
        #region Constants

        private const string ApiBaseAddress = "https://my.jasminsoftware.com/api";

        private const string AccountKey = "XXXXX"; // TODO: put your account key here
        private const string SubscriptionKey = "XXXXX-XXXX"; // TODO: put your account key here

        private const string AccessToken = "XXX"; // TODO: put the authorization access token here (this should be obtained previously)

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            // Start

            Console.WriteLine("Jasmin rocks! Let's list the orders using the Web API...");

            // Handle errors

            try
            {
                // Create the HTTP client to send the request

                using (HttpClient client = new HttpClient())
                {
                    // Set the authorization header

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                    // URI of the endpoint

                    string endpoint = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}/sales/orders", ApiBaseAddress, AccountKey, SubscriptionKey);

                    // Send the request (GET)

                    Console.WriteLine("Kicking the Orders endpoint to list the orders...");

                    Uri endpointUri = new Uri(endpoint);

                    using (HttpResponseMessage response = await client.GetAsync(endpointUri))
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

                        string json = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings settings = new JsonSerializerSettings()
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            Formatting = Formatting.Indented
                        };

                        IList<OrderResource> orders = JsonConvert.DeserializeObject<IList<OrderResource>>(json);

                        Console.WriteLine("The orders were obtained with success.");

                        foreach (OrderResource order in orders)
                        {
                            Console.WriteLine("Order: {0}.{1}.{2}.{3} (id = {4})", order.DocumentType, order.Company, order.Serie, order.SeriesNumber, order.Id);
                        }
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
