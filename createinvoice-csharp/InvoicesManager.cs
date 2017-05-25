using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jasmin.InvoiceSample
{
    /// <summary>
    /// The Jasmin invoice manager. Implements the jasmin invoice handling.
    /// </summary>
    public static class InvoicesManager
    {
        /// <summary>
        /// The Jasmin base application URL.
        /// </summary>
        public const string JasminBaseAppUrl = "https://my.jasminsoftware.com";

        /// <summary>
        /// The invoices resource path.
        /// </summary>
        public const string InvoicesResourcePath = "billing/invoices";

        /// <summary>
        /// Creates the invoice asynchronous.
        /// </summary>
        /// <param name="accountKey">The account key.</param>
        /// <param name="subscriptionKey">The subscription key.</param>
        /// <param name="token">The token.</param>
        /// <returns>The url for the created invoice.</returns>
        public static async Task<string> CreateInvoiceAsync(string accountKey, string subscriptionKey, string token)
        {
            // Parameter check
            if (string.IsNullOrEmpty(accountKey) || string.IsNullOrEmpty(subscriptionKey) || string.IsNullOrEmpty(token))
            {
                throw new ArgumentException();
            }

            string result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(JasminBaseAppUrl);

                    // define the authorization header

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // create the invoice

                    Invoice invoice = CreateInvoiceSample();

                    // define the invoice resource location

                    string resourceLocation = $"/api/{accountKey}/{subscriptionKey}/{InvoicesResourcePath}";

                    // define the JSON serialization to not include the null properties values.
                    JsonMediaTypeFormatter jsonFormat = new JsonMediaTypeFormatter();
                    jsonFormat.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                    jsonFormat.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                    // post the invoice object using the define JSON serialization

                    using (HttpResponseMessage responseContent = await client.PostAsync(resourceLocation, invoice, jsonFormat))
                    {
                        // check if the response is success 

                        if (responseContent.IsSuccessStatusCode)
                        {
                            // extract location header that contains the resource location created.

                            string location = responseContent?.Headers?.Location?.AbsoluteUri;

                            if (!string.IsNullOrEmpty(location))
                            {
                                Console.WriteLine($"Invoice Created: {location}");
                                result = location;
                            }
                        }
                        else
                        {
                            // the invoice creation has failed.

                            Console.WriteLine("Invoice Failed.");

                            // Check the error response.

                            string error = await responseContent.Content.ReadAsStringAsync();
                            Console.WriteLine(error);

                            throw new Exception("Unable to create invoice.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating invoice.", ex);
            }

            return result;
        }

        private static Invoice CreateInvoiceSample()
        {
            return new Invoice
            {
                // Company = "Default",
                // DocumentType = "FA",
                Customer = "0002",
                Lines = new List<InvoiceLine>
                    {
                        new InvoiceLine
                        {
                            Item = "0001",
                            Quantity = 2,
                            Description = "Item 0001 custom description.",
                            Price = new Money { Value = 50 }
                        }
                    }
            };
        }
    }
}