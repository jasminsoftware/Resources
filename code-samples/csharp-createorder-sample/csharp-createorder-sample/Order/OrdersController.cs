using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jasmin.OrderSample
{
    /// <summary>
    /// Defines a controller capable of managing orders.
    /// </summary>
    public static class OrdersController
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the account key.
        /// </summary>
        public static string AccountKey { get; set; }

        /// <summary>
        /// Gets or sets the subscription key.
        /// </summary>
        public static string SubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets the authorization token.
        /// </summary>
        public static string Token { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object that represents the asynchronous task.
        /// </returns>
        public static async Task CreateOrderAsync()
        {
            // Build the order that should be created

            Order order = new Order();

            try
            {
                order.Company = "Bocasuja";
                order.DocumentType = "ECL";
                order.DocumentDate = DateTime.UtcNow;
                order.CustomerId = "0002";
                order.Currency = "EUR";

                order.Lines = new List<OrderLine>
                {
                    new OrderLine
                    {
                        ItemId = "0002",
                        Quantity = 1,
                        Type = 1,

                        Price = new Price()
                        {
                            Value = 50,
                            Currency = "€"
                        }

                    }
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    // Build the request

                    string request = "sales/orders";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                    HttpRequestMessage postOrderMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    string invoiceRequest = JsonConvert.SerializeObject(order);

                    postOrderMessage.Content = new StringContent(invoiceRequest, Encoding.UTF8, "application/json");

                    // Send

                    using (HttpResponseMessage responseContent = await client.SendAsync(postOrderMessage))
                    {
                        // Get the response

                        if (responseContent.IsSuccessStatusCode)
                        {
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(string.Concat("Order created: ", result));
                        }
                        else
                        {
                            Console.WriteLine(string.Concat("Failed. ", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            Console.WriteLine(string.Concat("Content: ", result));

                            throw new Exception("Unable to create the order.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error creating the order.");
            }
        }

        #endregion
    }
}