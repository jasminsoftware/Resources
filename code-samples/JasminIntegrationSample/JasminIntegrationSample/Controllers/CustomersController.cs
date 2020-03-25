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
    /// Defines a controller capable of managing Customers.
    /// </summary>
    internal static class CustomersController
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
        /// Creates a new Party and Customer Extension (AllInOne) asynchronous.
        /// </summary>
        /// <returns>The key of the created party.</returns>
        public static async Task<string> CreateCustomerAndPartyAsync()
        {
            // Generate a PartyKey (only for sample purpose)

            string partyKey = $"NewC{DateTime.Now.ToString("yyyyMMddHHmmss")}";

            string partyTaxSchemaKey = await GetDefaultPartyTaxSchemaAsync();

            try
            {
                // Build the customer that will be created

                BaseCustomerResource newCustomer = new BaseCustomerResource()
                {
                    CustomerGroup = "01",
                    PaymentMethod = "NUM",
                    PaymentTerm = "00",
                    PartyTaxSchema = partyTaxSchemaKey,
                    Locked = false,
                    OneTimeCustomer = false,
                    EndCustomer = false,
                    PartyKey = partyKey,
                    SearchTerm = partyKey,
                    Name = partyKey,
                    StreetName = "Avenida de Ceuta",
                    BuildingNumber = "1000",
                    CityName = "Lisboa",
                    PostalZone = "1010-023",
                    Telephone = "219877890"
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salesCore/customerParties";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postCustomerMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    // Build the json data

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string customerContent = JsonConvert.SerializeObject(newCustomer, settings);
                    postCustomerMessage.Content = new StringContent(customerContent, Encoding.UTF8, "application/json");

                    // Log Request

                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", customerContent);

                    // Send

                    using (HttpResponseMessage customerResponse = await client.SendAsync(postCustomerMessage))
                    {
                        // Get the response

                        if (!customerResponse.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", customerResponse.ToString()));
                            string errorResult = await ((StreamContent)customerResponse.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));
                            partyKey = string.Empty;

                            throw new Exception(string.Format("Unable to create the Customer '{0}'.", partyKey));
                        }

                        // Succeeded

                        string result = await ((StreamContent)customerResponse.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Customer '{0}' created: (Id = {1})", partyKey, result));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                partyKey = string.Empty;
                throw new Exception(string.Format("Error creating the Customer '{0}'.", partyKey));
            }

            return partyKey;
        }

        /// <summary>
        /// Creates a Customer Extension asynchronous.
        /// </summary>
        /// <returns>The key of the created party.</returns>
        public static async Task<string> CreatePartyAndCustomerExtensionAsync()
        {
            // For Sample purpose We will Create a Party And than create the Customer Extension.
            // Usefull when you allread have the Party and only want to add the customer extension. 
            // (The same occurrs for all the other extensions).

            string partyKey = $"NewP{DateTime.Now.ToString("yyyyMMddHHmmss")}";

            string partyTaxSchemaKey = await GetDefaultPartyTaxSchemaAsync();

            try
            {
                // Build the party that will be created

                PartyResource newParty = new PartyResource()
                {
                    PartyKey = partyKey,
                    SearchTerm = partyKey,
                    Name = "New Party",
                    CityName = "Porto", 
                    PostalZone = "4000-047",
                    Telephone = "223123345",
                    StreetName = "Rua Santa Catarina",
                    BuildingNumber = "150 RC"
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "businessCore/parties";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postPartyMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    // Build the json data

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string partyContent = JsonConvert.SerializeObject(newParty, settings);
                    postPartyMessage.Content = new StringContent(partyContent, Encoding.UTF8, "application/json");

                    // Log Request

                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", partyContent);

                    // Send

                    using (HttpResponseMessage partyResponse = await client.SendAsync(postPartyMessage))
                    {
                        // Get the response

                        if (!partyResponse.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", partyResponse.ToString()));
                            string errorResult = await ((StreamContent)partyResponse.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));
                            partyKey = string.Empty;

                            throw new Exception(string.Format("Unable to create the Party '{0}'", partyKey));
                        }

                        // Succeeded

                        string result = await ((StreamContent)partyResponse.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Party '{0}' created : (Id = {1})", partyKey, result));

                        // Build the customer that will be created

                        CustomerResource newCustomer = new CustomerResource()
                        {
                            CustomerGroup = "01",
                            PaymentMethod = "NUM",
                            PaymentTerm = "00",
                            PartyTaxSchema = partyTaxSchemaKey,
                            Locked = false,
                            OneTimeCustomer = false,
                            EndCustomer = false,
                            BaseEntityKey = partyKey
                        };

                        // Build the request

                        request = "salesCore/customerParties/extension";
                        resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                        client.SetDefaultRequestHeaders(CultureKey);

                        // It's a POST

                        HttpRequestMessage postCustomerMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                        // Build the json data

                        string customerContent = JsonConvert.SerializeObject(newCustomer, settings);
                        postCustomerMessage.Content = new StringContent(customerContent, Encoding.UTF8, "application/json");

                        // Log Request

                        Console.WriteLine("Request - POST");
                        Console.WriteLine("{0}", resourceLocation);
                        Console.WriteLine("Request - BODY ");
                        Console.WriteLine("{0}", customerContent);

                        // Send

                        using (HttpResponseMessage customerResponse = await client.SendAsync(postCustomerMessage))
                        {
                            // Get the response

                            if (!customerResponse.IsSuccessStatusCode)
                            {
                                ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", customerResponse.ToString()));
                                string resultError = await ((StreamContent)customerResponse.Content).ReadAsStringAsync();
                                ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", resultError));
                                partyKey = string.Empty;

                                throw new Exception(string.Format("Unable to create the Customer '{0}'.", partyKey));
                            }

                            // Succeeded

                            result = await ((StreamContent)customerResponse.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteSuccessLine(string.Format("Customer '{0}' created: (Id = {1})", partyKey, result));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);

                throw new Exception(string.Format("Error creating the Customer '{0}'", partyKey));
            }

            return partyKey;
        }

        /// <summary>
        /// Gets the last created customer asynchronous.
        /// </summary>
        /// <param name="silentMode">if set to <c>true</c> console messages will not be displayed.</param>
        /// <returns>The key of the last created customer.</returns>
        public static async Task<string> GetLastCustomerAsync(bool silentMode = false)
        {
            string foundPartyKey = null;
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salescore/customerParties/odata";

                    // build the odata expression

                    string odataExpression = "?$select=PartyKey, PaymentTerm&$top=1&$filter= IsActive eq true and IsSystem eq false and IsDeleted eq false&$orderby=CreatedOn desc";

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    if (!silentMode)
                    {
                        // Log Request

                        Console.WriteLine("Request - GET");
                        Console.WriteLine("{0}", resourceLocation);
                    }

                    // Send

                    using (HttpResponseMessage responseContent = await client.GetAsync(resourceLocation))
                    {
                        // Get the response

                        if (responseContent.IsSuccessStatusCode)
                        {
                            string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                            var objectResult = JsonConvert.DeserializeObject<ODataResponse<BaseCustomerResource>>(json);
                            IList<BaseCustomerResource> customers = objectResult.Items;

                            foundPartyKey = customers[0].PartyKey;

                            if (!silentMode)
                            {
                                ConsoleHelper.WriteSuccessLine(string.Format("The last customer was found with success. ('{0}' - PaymentTerm:{1})", foundPartyKey, customers[0].PaymentTerm));
                                Console.WriteLine("");
                            }
                        }
                        else
                        {
                            if (!silentMode)
                            {
                                ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                                string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                                ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                                throw new Exception("Unable to get the last customer.");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error getting the last customer.");
            }

            return foundPartyKey;
        }

        /// <summary>
        /// Updates the customer city address asynchronous.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public static async Task UpdateCustomerCityAddressAsync(string customer)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(customer))
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("UpdateCustomerCityAddress: 'customer' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }

            // City is not really an attribute of the customer.
            // Its an attribute of the maid entity Party.
            // So we need to set it on Party
            // Generate city name for sample purpose only.

            string newCityName = $"NewCity {DateTime.Now.ToString("yyyyMMddHHmmss")}";

            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request
                    string request = string.Format("businessCore/parties/{0}/cityName", customer);
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request, customer);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a PUT

                    HttpRequestMessage putCustomerMessage = new HttpRequestMessage(HttpMethod.Put, resourceLocation);

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string customerContent = JsonConvert.SerializeObject(newCityName, settings);

                    putCustomerMessage.Content = new StringContent(customerContent, Encoding.UTF8, "application/json");

                    // Log Request

                    Console.WriteLine("Request - PUT");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", customerContent);

                    // Send

                    using (HttpResponseMessage responseContent = await client.SendAsync(putCustomerMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception(string.Format("Unable to update city name on Customer '{0}'.", customer));
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Updated city name on Customer '{0}')", customer));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error updating city name on Customer '{0}'", customer));
            }
        }

        /// <summary>
        /// Updates the customer payment term asynchronous.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public static async Task UpdateCustomerPaymentTermAsync(string customer)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(customer))
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("UpdateCustomerPaymentTermAsync: 'customer' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }

            // For sample purpose we will set then payment term to 90 days,

            string newPaymentTerm = "03";

            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request
                    string request = string.Format("salesCore/customerParties/{0}/paymentTerm", customer);
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request, customer);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a PUT

                    HttpRequestMessage putCustomerMessage = new HttpRequestMessage(HttpMethod.Put, resourceLocation);

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string customerContent = JsonConvert.SerializeObject(newPaymentTerm, settings);

                    putCustomerMessage.Content = new StringContent(customerContent, Encoding.UTF8, "application/json");

                    // Log Request

                    Console.WriteLine("Request - PUT");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", customerContent);

                    // Send

                    using (HttpResponseMessage responseContent = await client.SendAsync(putCustomerMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception(string.Format("Unable to update payment term on Customer '{0}'", customer));
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Updated payment term on Customer '{0}')", customer));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error updating payment term on Customer '{0}", customer));
            }
        }

        /// <summary>
        /// Deletes the customer extension asynchronous.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public static async Task DeleteCustomerExtensionAsync(string customer)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(customer))
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("DeleteCustomerExtensionAsync: 'customer' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }

            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = string.Format("salesCore/customerParties/{0}", customer);
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a DELETE

                    HttpRequestMessage deleteCustomerMessage = new HttpRequestMessage(HttpMethod.Delete, resourceLocation);

                    // Log Request

                    Console.WriteLine("Request - DEL");
                    Console.WriteLine("{0}", resourceLocation);

                    // Send

                    using (HttpResponseMessage responseContent = await client.SendAsync(deleteCustomerMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception(string.Format("Unable to delete the customer extension for Party '{0}", customer));
                        }

                        // Succeeded

                        ConsoleHelper.WriteSuccessLine(string.Format("Customer Extension for Party '{0}' was deleted", customer));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error deleting the customer extension for Party '{0}'", customer));
            }
        }

        /// <summary>
        /// Creates a Customer Extension.
        /// </summary>
        /// <returns>The task.</returns>
        public static async Task CreateCustomerExtensionAsync(string customer)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(customer))
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("CreateCustomerExtensionAsync: 'customer' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }

            string partyTaxSchemaKey = await GetDefaultPartyTaxSchemaAsync();

            try
            {
                // Build the customer that will be created

                CustomerResource newCustomer = new CustomerResource()
                {
                    CustomerGroup = "01",
                    PaymentMethod = "NUM",
                    PaymentTerm = "00",
                    PartyTaxSchema = partyTaxSchemaKey,
                    Locked = false,
                    OneTimeCustomer = false,
                    EndCustomer = false,
                    BaseEntityKey = customer
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salesCore/customerParties/extension";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postCustomerMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    // Build the json data

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string customerContent = JsonConvert.SerializeObject(newCustomer, settings);
                    postCustomerMessage.Content = new StringContent(customerContent, Encoding.UTF8, "application/json");

                    // Log Request

                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", customerContent);

                    // Send

                    using (HttpResponseMessage responseContent = await client.SendAsync(postCustomerMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception(string.Format("Unable to create the customer extention for Party'{0}'.", customer));
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Customer extension created for Party '{0}': (Id = {1})", customer, result));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error creating the customer extension for Party '{0}'.", customer));
            }
        }

        /// <summary>
        /// Deletes all parties created on this sample.
        /// </summary>
        public static async Task DeleteAllPartiesCreatedOnSample()
        {
            await DeleteAllCustomerPartiesCreatedOnSample();
            await DeleteAllBaseCustomersCreatedOnSample();
        }

        #endregion

        #region Private Methods

        private static async Task<string> GetDefaultPartyTaxSchemaAsync()
        {
            string foundPartyTaxSchema = null;
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salescore/customerGroups/odata";

                    // build the odata expression

                    string odataExpression = "?$select=PartyTaxSchema&$top=1&$filter= CustomerGroupKey eq '01' and IsActive eq true and IsSystem eq false and IsDeleted eq false";

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Send

                    using (HttpResponseMessage responseContent = await client.GetAsync(resourceLocation))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to get the default PartyTaxSchema.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<CustomerGroupResource>>(json);
                        IList<CustomerGroupResource> customerGroups = objectResult.Items;

                        foundPartyTaxSchema = customerGroups[0].PartyTaxSchema;
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error getting the default PartyTaxSchema.");
            }

            return foundPartyTaxSchema;
        }

        private static async Task DeleteAllCustomerPartiesCreatedOnSample()
        {
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salescore/customerParties/odata";

                    // build the odata expression

                    string partyKey = $"NewC{DateTime.Now.ToString("yyyyMMddHHmmss")}".Substring(0,8);
                    string otherPartyKey = $"NewP{DateTime.Now.ToString("yyyyMMddHHmmss")}".Substring(0, 8);

                    string odataExpression = string.Format("?$select=PartyKey&$filter= IsActive eq true and IsSystem eq false and IsDeleted eq false and (startswith(PartyKey, '{0}') or startswith(PartyKey, '{1}')) eq true and startswith(CreatedBy, 'application-user::')", partyKey, otherPartyKey);

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Log Request

                    Console.WriteLine("");
                    Console.WriteLine("Get All Customers to delete");
                    Console.WriteLine("");
                    Console.WriteLine("Request - GET");
                    Console.WriteLine("{0}", resourceLocation);

                    // Send

                    using (HttpResponseMessage responseContent = await client.GetAsync(resourceLocation))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to get the customers to delete.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<CustomerResource>>(json);
                        IList<CustomerResource> customers = objectResult.Items;

                        // Log 

                        if (customers.Count == 0)
                        {
                            Console.WriteLine("");
                            ConsoleHelper.WriteSuccessLine("No customers found to delete");
                            Console.WriteLine("");
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Delete the customers found");
                            Console.WriteLine("");

                            foreach (CustomerResource foundCustomer in customers)
                            {
                                string foundCustomerKey = foundCustomer.PartyKey;

                                // Build the request

                                request = string.Format("salesCore/customerParties/{0}", foundCustomerKey);
                                resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                                client.SetDefaultRequestHeaders(CultureKey);

                                // It's a DELETE

                                HttpRequestMessage deleteCustomerMessage = new HttpRequestMessage(HttpMethod.Delete, resourceLocation);

                                // Log Request

                                Console.WriteLine("Request - DEL");
                                Console.WriteLine("{0}", resourceLocation);

                                // Send

                                using (HttpResponseMessage deleteResponseContent = await client.SendAsync(deleteCustomerMessage))
                                {
                                    // Get the response

                                    if (!deleteResponseContent.IsSuccessStatusCode)
                                    {
                                        ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", deleteResponseContent.ToString()));
                                        string result = await ((StreamContent)deleteResponseContent.Content).ReadAsStringAsync();
                                        ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                                        throw new Exception(string.Format("Unable to delete the customer extension for Party '{0}'.", foundCustomerKey));
                                    }

                                    // Succeeded

                                    ConsoleHelper.WriteSuccessLine(string.Format("Customer extension for Party '{0}' was deleted", foundCustomerKey));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error deleting the customers created by this sample.");
            }
        }

        private static async Task DeleteAllBaseCustomersCreatedOnSample()
        {
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "businessCore/parties/odata";

                    // build the odata expression

                    string partyKey = $"NewC{DateTime.Now.ToString("yyyyMMddHHmmss")}".Substring(0, 8);
                    string otherPartyKey = $"NewP{DateTime.Now.ToString("yyyyMMddHHmmss")}".Substring(0, 8);

                    string odataExpression = string.Format("?$select=PartyKey&$filter= IsActive eq true and IsSystem eq false and IsDeleted eq false and (startswith(PartyKey, '{0}') or startswith(PartyKey, '{1}')) eq true and startswith(CreatedBy, 'application-user::')", partyKey, otherPartyKey);

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Log Request

                    Console.WriteLine("");
                    Console.WriteLine("Get All Parties to delete");
                    Console.WriteLine("");
                    Console.WriteLine("Request - GET");
                    Console.WriteLine("{0}", resourceLocation);

                    // Send

                    using (HttpResponseMessage responseContent = await client.GetAsync(resourceLocation))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to get the parties cretaed by this sample.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<PartyResource>>(json);
                        IList<PartyResource> parties = objectResult.Items;

                        // Log
                        if (parties.Count == 0)
                        {
                            Console.WriteLine("");
                            ConsoleHelper.WriteSuccessLine("No parties found to delete");
                            Console.WriteLine("");
                        }
                        else
                        {

                            Console.WriteLine("");
                            Console.WriteLine("Delete the parties found");
                            Console.WriteLine("");

                            foreach (PartyResource foundParty in parties)
                            {
                                string foundPartyKey = foundParty.PartyKey;

                                // Build the request

                                request = string.Format("businessCore/parties/{0}", foundPartyKey);
                                resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                                client.SetDefaultRequestHeaders(CultureKey);

                                // It's a DELETE

                                HttpRequestMessage deleteCustomerMessage = new HttpRequestMessage(HttpMethod.Delete, resourceLocation);

                                // Log Request

                                Console.WriteLine("Request - DEL");
                                Console.WriteLine("{0}", resourceLocation);

                                // Send

                                using (HttpResponseMessage deleteResponseContent = await client.SendAsync(deleteCustomerMessage))
                                {
                                    // Get the response

                                    if (!deleteResponseContent.IsSuccessStatusCode)
                                    {
                                        ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", deleteResponseContent.ToString()));
                                        string result = await ((StreamContent)deleteResponseContent.Content).ReadAsStringAsync();
                                        ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                                        ConsoleHelper.WriteErrorLine(string.Format("Unable to delete the Party '{0}'.", foundPartyKey));
                                    }
                                    else
                                    {
                                        // Succeeded

                                        ConsoleHelper.WriteSuccessLine(string.Format("Party '{0}' was deleted", foundPartyKey));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error deleting all parties created by this sample.");
            }
        }

        #endregion
    }
}
