using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ItemExtensions
{
    public static class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            // Start

            Console.WriteLine("Jasmin rocks! Let's create an item and sales item using the Web API...");

            // Login - Client Credentials Flow

            Console.WriteLine("Let's start by requesting the access token...");

            AuthenticationProvider authenticationProvider = new AuthenticationProvider(Constants.Application.ClientId, Constants.Application.ClientSecret);

            // Handle errors

            try
            {
                // Create the HTTP client to send the request

                using (HttpClient client = new HttpClient())
                {
                    // Set the authorization header

                    await authenticationProvider.SetAccessTokenAsync(client);

                    Console.WriteLine("Kicking method 1: Creating the item and then add the sales item extension... ");
                    // 1. In this first method, we will create an item and then add a sales item extension
                    await CreateItemAndAddSalesItemExtension(client);

                    Console.WriteLine("Kicking method 2: Creating the sales item and the item at the same time.... ");
                    // 2. In this second method, we will create the item and the sales item at the same time
                    await CreateSalesItem(client);
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

        private static async Task CreateItemAndAddSalesItemExtension(HttpClient client)
        {
            // URI of the endpoint

            string endpoint = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}/businessCore/items", Constants.Jasmin.ApiBaseAddress, Constants.Jasmin.AccountKey, Constants.Jasmin.SubscriptionKey);

            // Send the request (POST)

            Console.WriteLine("Kicking the Items endpoint to create a new item...");

            Uri endpointUri = new Uri(endpoint);

            string itemKey = $"Y{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            ItemResource item = new ItemResource()
            {
                ItemKey = itemKey,
                BaseUnit = "KG",
                ItemType = 1,
                Unit = "KG",
                ItemTaxSchema = "IVA-TN",
            };

            using (HttpResponseMessage response = await client.PostAsync(endpointUri, new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")))
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

                Console.WriteLine("The item was created with success.");

                // Add the Sales Item extension to the previously created item

                Console.WriteLine("Kicking the Sales Item endpoint to add a new sales item extension to the item...");

                SalesItemResource salesItem = new SalesItemResource()
                {
                    BaseEntityKey = itemKey, // BaseEntityKey: points to the Item.NaturalKey attribute and is used to mark this Sales Item instance has an extension of the Item
                    Unit = "KG",
                    IncomeAccount = "71111",
                    ItemTaxSchema = "IVA-TN",
                };

                // URI of the endpoint

                string salesItemEndpoint = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}/salesCore/salesItems/extension", Constants.Jasmin.ApiBaseAddress, Constants.Jasmin.AccountKey, Constants.Jasmin.SubscriptionKey);
                Uri salesItemEndpointUri = new Uri(salesItemEndpoint);

                using (HttpResponseMessage salesItemResponse = await client.PostAsync(salesItemEndpointUri, new StringContent(JsonConvert.SerializeObject(salesItem), Encoding.UTF8, "application/json")))
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

                    Console.WriteLine("The sales item was created with success.");
                }
            }
        }

        private static async Task CreateSalesItem(HttpClient client)
        {
            // URI of the endpoint

            string endpoint = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}/salesCore/salesItems", Constants.Jasmin.ApiBaseAddress, Constants.Jasmin.AccountKey, Constants.Jasmin.SubscriptionKey);

            // Send the request (POST)

            Console.WriteLine("Kicking the Sales Items endpoint to create a new sales item and item...");

            Uri endpointUri = new Uri(endpoint);

            string itemKey = $"Z{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            SalesItemResource salesItem = new SalesItemResource()
            {
                ItemKey = itemKey,
                Unit = "KG",
                IncomeAccount = "71111",
                ItemTaxSchema = "IVA-TN",
            };

            using (HttpResponseMessage response = await client.PostAsync(endpointUri, new StringContent(JsonConvert.SerializeObject(salesItem), Encoding.UTF8, "application/json")))
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

                Console.WriteLine("The item and sales item were created with success.");
            }
        }

        #endregion
    }
}
