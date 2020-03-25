using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jasmin.IntegrationSample.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jasmin.IntegrationSample
{
    /// <summary>
    /// Defines a controller capable of managing Logistics Setup.
    /// </summary>
    public static class LogisticsSetupsController
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
        /// Gets or sets the culture key.
        /// </summary>
        public static string CultureKey { get; set; }

        /// <summary>
        /// Gets or sets the authentication provider.
        /// </summary>
        internal static AuthenticationProvider AuthenticationProvider { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Allows the negative stock on company asynchronous.
        /// </summary>
        /// <param name="companyKey">The company key.</param>
        public static async Task AllowNegativeStockOnCompanyAsync(string companyKey)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(companyKey))
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("AllowNegativeStockAsync: 'companyKey' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }

            bool allowNegativeStock = true;

            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request
                    string request = "logisticsCore/logisticsSetups";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/{4}/negativeStock", Constants.baseAppUrl, AccountKey, SubscriptionKey, request, companyKey);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a PUT

                    HttpRequestMessage putStockMessage = new HttpRequestMessage(HttpMethod.Put, resourceLocation);

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string stockContent = JsonConvert.SerializeObject(allowNegativeStock, settings);

                    putStockMessage.Content = new StringContent(stockContent, Encoding.UTF8, "application/json");

                    // Send

                    Console.WriteLine("Request - PUT");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", stockContent);

                    using (HttpResponseMessage responseContent = await client.SendAsync(putStockMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception(string.Format("Unable to allow negative stock on Company '{0}'.", companyKey));
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Negative stock has been allowed on Company '{0}'.", companyKey));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error allowing negative stock on Company '{0}'.", companyKey));
            }
        }

        #endregion
    }
}