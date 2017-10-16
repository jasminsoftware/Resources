using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jasmin.OrderSample
{
    /// <summary>
    /// The static class that holds the application starting point.
    /// </summary>
    public static class Program
    {
        #region Public Methods

        /// <summary>
        /// The starting point of the application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            // Start the async task

            try
            {
                Console.WriteLine("Starting ...");

                DoItAsync().Wait();

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        #endregion

        #region Private Methods

        private static async Task<int> DoItAsync()
        {
            // Get the authorization access token

            string accessToken = await GetAccessTokenAsync();
            Console.WriteLine(accessToken != null ? "Authorization token found." : "Authorization token not found.");

            if (accessToken != null)
            {
                // Get the accoun key

                Console.Write("Insert Account Key: ");
                string account = Console.ReadLine();

                // Get the subscription key

                Console.Write("Insert Subscription Key: ");
                var subscription = Console.ReadLine();

                OrdersController.AccountKey = account;
                OrdersController.SubscriptionKey = subscription;
                OrdersController.Token = accessToken;

                // Create the order

                await OrdersController.CreateOrderAsync();
            }

            return 0;
        }

        private static async Task<string> GetAccessTokenAsync()
        {
            // Get the authorization token using the client credentials grant type

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Console.WriteLine("Requesting the access token from the authorization server...");

                    client.BaseAddress = new Uri(Constants.baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Build the request data (grant type client credentials)

                    List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                    postData.Add(new KeyValuePair<string, string>("scope", "application"));
                    postData.Add(new KeyValuePair<string, string>("client_id", Constants.clientId));
                    postData.Add(new KeyValuePair<string, string>("client_secret", Constants.clientSecret));

                    FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                    // Post the request and get the response

                    HttpResponseMessage response = await client.PostAsync(Constants.baseUrl, content);
                    string jsonString = await response.Content.ReadAsStringAsync();
                    object responseData = JsonConvert.DeserializeObject(jsonString);

                    // The access token in the response

                    return ((dynamic)responseData).access_token;

                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error getting token. {0}", ex.Message));
                }
            }
        }

        #endregion
    }
}
