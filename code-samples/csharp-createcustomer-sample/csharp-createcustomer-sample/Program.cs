using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CreateCustomer
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

            Console.WriteLine("Jasmin rocks! Let's create a customer using the Web API...");

            // Handle errors

            try
            {
                // Create the HTTP client to send the request

                using (HttpClient client = new HttpClient())
                {
                    // Set the authorization header

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                    // Build the customer that will be created

                    CustomerResource resource = new CustomerResource()
                    {
                        CustomerGroup = "01",
                        PaymentMethod = "NUM",
                        PaymentTerm = "00",
                        PartyTaxSchema = "IVA-RN-MN",
                        Locked = false,
                        OneTimeCustomer = false,
                        EndCustomer = false,
                        PartyKey = "NewCustomer",
                        SearchTerm = "NewCustomer",
                        Name = "New Customer"
                    };

                    // Build the json data

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string json = JsonConvert.SerializeObject(resource, settings);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    // URI of the endpoint

                    string endpoint = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}/salesCore/customerParties", ApiBaseAddress, AccountKey, SubscriptionKey);

                    // Send the request (POST)

                    Console.WriteLine("Kicking the CustomerParties endpoint to create the customer...");

                    Uri endpointUri = new Uri(endpoint);

                    using (HttpResponseMessage response = await client.PostAsync(endpointUri, content))
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

                        Console.WriteLine("The customer was created with success.");
                        Console.WriteLine("The new customer id is {0}.", customerId);
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
