﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jasmin.IntegrationSample.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jasmin.IntegrationSample
{
    /// <summary>
    /// Defines a controller capable of managing companies.
    /// </summary>
    public static class CompaniesController
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

        public static async Task<string> GetCompanyKeyAsync()
        {
            string companyKey = string.Empty;

            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    // SAMPLE: Get the companyKey for the first active existing company
                    Console.WriteLine("");
                    Console.WriteLine("As orders are company dependent entities, for this sample, we will need to"); 
                    Console.WriteLine("get the company key where we will be acting upon");

                    string request = "corepatterns/companies/odata?$select=CompanyKey&$filter=IsActive eq true and IsSystem eq false and IsDeleted eq false &$top=1&$orderby=CreatedOn";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a GET

                    HttpRequestMessage getOrderMessage = new HttpRequestMessage(HttpMethod.Get, resourceLocation);

                    // Send

                    Console.WriteLine("Request - GET");
                    Console.WriteLine("{0}", resourceLocation);


                    using (HttpResponseMessage responseContent = await client.SendAsync(getOrderMessage))
                    {
                        // Get the response

                        if (responseContent.IsSuccessStatusCode)
                        {
                            string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                            JsonSerializerSettings settings = new JsonSerializerSettings()
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                Formatting = Formatting.Indented
                            };

                            var objectResult = JsonConvert.DeserializeObject<ODataResponse<CompanyResource>>(json);
                            IList<CompanyResource> companies = objectResult.Items;
                            
                            companyKey = companies[0].CompanyKey;

                            ConsoleHelper.WriteSuccessLine(String.Format("Company Key: {0}", companyKey));
                        }
                        else
                        {
                            ConsoleHelper.WriteErrorLine(String.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(String.Format("Content: {0}", result));

                            throw new Exception("Unable to get the company Key.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error geting the current company.");
            }

            return companyKey;
        }

        #endregion
    }
}