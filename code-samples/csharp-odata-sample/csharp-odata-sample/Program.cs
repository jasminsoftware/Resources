using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http.OData;

namespace ListOrders
{
    public static class Program
    {
        #region Constants

        private const string ApiBaseAddress = "https://my.jasminsoftware.com/api";

        private const string AccountKey = "xxx"; // TODO: put your account key here
        private const string SubscriptionKey = "xxxxx-xxxx"; // TODO: put your account key here

        private const string AccessToken = "xxxxxxxxxxxxxxx"; // TODO: put the authorization access token here (this should be obtained previously)

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            // Start

            Console.WriteLine("Jasmin rocks! Let's list the orders attributes using the OData...");

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

                    string endpoint = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}/sales/orders/odata?$inlinecount=allpages&$select=DocumentType,SeriesNumber,Company&$top=10&$orderby=Serie desc", ApiBaseAddress, AccountKey, SubscriptionKey);

                    // Send the request (GET)

                    Console.WriteLine("Kicking the Orders OData endpoint to list the orders attributes...");

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

                        PageResult<OrderResource> orders = JsonConvert.DeserializeObject<PageResult<OrderResource>>(json);

                        Console.WriteLine("The orders attributes were obtained with success.");

                        foreach (OrderResource order in orders.Items)
                        {
                            Console.WriteLine("Order: {0}.{1}.{2}", order.DocumentType, order.Company, order.SeriesNumber);
                        }
                        Console.WriteLine("Count: {0}", orders.Count);
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
