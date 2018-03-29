using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GetInvoices
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

            Console.WriteLine("Jasmin rocks! Let's create a sales invoice using the Web API...");

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

                    // URI of the endpoint

                    string endpoint = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}/billing/invoices", Constants.Jasmin.ApiBaseAddress, Constants.Jasmin.AccountKey, Constants.Jasmin.SubscriptionKey);

                    // Send the request (POST)

                    Console.WriteLine("Kicking the Invoices endpoint to list the invoices...");

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

                        IList<InvoiceResource> invoices = JsonConvert.DeserializeObject<IList<InvoiceResource>>(json);

                        Console.WriteLine("The invoices were obtained with success.");

                        foreach (InvoiceResource invoice in invoices)
                        {
                            Console.WriteLine("Invoice: {0}.{1}.{2}.{3} (id = {4})", invoice.DocumentType, invoice.Company, invoice.Serie, invoice.SeriesNumber, invoice.Id);
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
