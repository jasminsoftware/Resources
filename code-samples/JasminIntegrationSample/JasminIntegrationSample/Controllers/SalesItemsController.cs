using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jasmin.IntegrationSample.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jasmin.IntegrationSample
{
    /// <summary>
    /// Defines a controller capable of managing SalesItems.
    /// </summary>
    public static class SalesItemsController
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
        /// Creates a new Item and Sales Item Extension (AllInOne) asynchronous.
        /// </summary>
        /// <returns>The key of the created item.</returns>
        public static async Task<string> CreateSalesItemAndItemAsync()
        {
            // Generate a ItemKey (only for sample purpose)

            string itemKey = $"NewS{DateTime.Now.ToString("yyyyMMddHHmmss")}";

            string itemTaxSchemaKey = await GetDefaultItemTaxSchemaAsync();

            try
            {
                // Build the customer that will be created

                BaseSalesItemResource customer = new BaseSalesItemResource()
                {
                    ItemKey = itemKey,
                    Description = itemKey,
                    BaseUnit = "MT",
                    Unit = "KM",
                    ItemTaxSchema = itemTaxSchemaKey,
                    IncomeAccount = "71111",
                    ItemType = "Item"
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salesCore/salesItems";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postCustomerMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    // Build the json data

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string salesItemContent = JsonConvert.SerializeObject(customer, settings);
                    postCustomerMessage.Content = new StringContent(salesItemContent, Encoding.UTF8, "application/json");

                    // Log Request

                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", salesItemContent);

                    // Send

                    using (HttpResponseMessage responseContent = await client.SendAsync(postCustomerMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            itemKey = string.Empty;

                            throw new Exception(string.Format("Unable to create the SalesItem '{0}'.", itemKey));
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("SalesItem '{0}' created: (Id = {1})", itemKey, result));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error creating the SalesItem '{0}'.", itemKey));
            }

            return itemKey;
        }

        /// <summary>
        /// Creates a SalesItem Extension asynchronous.
        /// </summary>
        /// <returns>The key of the created item.</returns>
        public static async Task<string> CreateItemAndSalesItemExtensionAsync()
        {
            // For Sample purpose We will Create an Item And than create the SalesItem Extension.
            // Usefull when you allread have the Item and only want to add the salesitem extension. 
            // (The same occurrs for all the other extensions).

            string itemKey = $"NewI{DateTime.Now.ToString("yyyyMMddHHmmss")}";

            string itemTaxSchemaKey = await GetDefaultItemTaxSchemaAsync();

            try
            {
                // Build the item that will be created

                ItemResource newItem = new ItemResource()
                {
                    ItemKey = itemKey,
                    ItemType = "Item",
                    BaseUnit = "MT",
                    Description = itemKey
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "businessCore/items";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postItemMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    // Build the json data

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string itemContent = JsonConvert.SerializeObject(newItem, settings);
                    postItemMessage.Content = new StringContent(itemContent, Encoding.UTF8, "application/json");

                    // Log Request

                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", itemContent);

                    // Send

                    using (HttpResponseMessage itemResponse = await client.SendAsync(postItemMessage))
                    {
                        // Get the response

                        if (!itemResponse.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", itemResponse.ToString()));
                            string errorResult = await ((StreamContent)itemResponse.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));
                            itemKey = string.Empty;

                            throw new Exception(string.Format("Unable to create the Item '{0}'", itemKey));
                        }

                        // Succeeded

                        string result = await ((StreamContent)itemResponse.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Item '{0}' created: (Id = {1})", itemKey, result));

                        // Build the salesItems that will be created

                        SalesItemResource newSalesItems = new SalesItemResource()
                        {
                            IncomeAccount = "71111",
                            BaseEntityKey = itemKey,
                            Unit = "KM",
                            ItemTaxSchema = itemTaxSchemaKey
                        };

                        // Build the request

                        request = "salesCore/salesItems/extension";
                        resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                        client.SetDefaultRequestHeaders(CultureKey);

                        // It's a POST

                        HttpRequestMessage postSalesItemsMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                        // Build the json data

                        string salesItemsContent = JsonConvert.SerializeObject(newSalesItems, settings);
                        postSalesItemsMessage.Content = new StringContent(salesItemsContent, Encoding.UTF8, "application/json");

                        // Log Request

                        Console.WriteLine("Request - POST");
                        Console.WriteLine("{0}", resourceLocation);
                        Console.WriteLine("Request - BODY ");
                        Console.WriteLine("{0}", salesItemsContent);

                        // Send

                        using (HttpResponseMessage salesItemsResponse = await client.SendAsync(postSalesItemsMessage))
                        {
                            // Get the response

                            if (!salesItemsResponse.IsSuccessStatusCode)
                            {
                                ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", salesItemsResponse.ToString()));
                                string resultError = await ((StreamContent)salesItemsResponse.Content).ReadAsStringAsync();
                                ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", resultError));
                                itemKey = string.Empty;

                                throw new Exception(string.Format("Unable to create the SalesItems '{0}'.", itemKey));
                            }

                            // Succeeded

                            result = await ((StreamContent)salesItemsResponse.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteSuccessLine(string.Format("SalesItems '{0}' created: (Id = {1})", itemKey, result));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);

                itemKey = string.Empty;
                throw new Exception(string.Format("Error creating the SalesItems"));
            }

            return itemKey;
        }

        /// <summary>
        /// Gets the last created salesItem asynchronous.
        /// </summary>
        /// <param name="silentMode">if set to <c>true</c> console messages will not be displayed.</param>
        /// <returns>The key of the last created salesItem.</returns>
        public static async Task<string> GetLastCreatedSalesItemAsync(bool silentMode = false)
        {
            string foundItemKey = null;
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salescore/salesItems/odata";

                    // build the odata expression

                    string odataExpression = "?$top=1&$filter= IsActive eq true and IsSystem eq false and IsDeleted eq false&$orderby=CreatedOn desc";

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

                            var objectResult = JsonConvert.DeserializeObject<ODataResponse<BaseSalesItemResource>>(json);
                            IList<BaseSalesItemResource> salesItems = objectResult.Items;

                            BaseSalesItemResource foundSalesItem = salesItems[0];
                            foundItemKey = foundSalesItem.ItemKey;

                            if (!silentMode)
                            {
                                ConsoleHelper.WriteSuccessLine(string.Format("The last salesItem was found with success. ('{0}')", foundItemKey));
                                Console.WriteLine("");
                                if (foundSalesItem.PriceListLines.Count > 0)
                                {
                                    Console.WriteLine("Prices List");
                                    foreach (SalesItemPriceListLineResource line in foundSalesItem.PriceListLines)
                                    {
                                        Console.WriteLine(string.Format("PriceList '{0}', Unit '{1}', Price {2:N}{3}", line.PriceList, line.Unit, line.Price.Value, line.Price.Currency));
                                    }
                                    Console.WriteLine("");
                                }
                            }
                        }
                        else
                        {
                            if (!silentMode)
                            {
                                ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                                string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                                ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                                throw new Exception("Unable to get the last salesItem.");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error getting the last salesItem.");
            }

            return foundItemKey;
        }

        /// <summary>
        /// Inserts the price on sales item asynchronous.
        /// </summary>
        public static async Task InsertPriceOnSalesItemAsync(string itemKey)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(itemKey))
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("InsertPriceOnSalesItemAsync: 'itemKey' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }

            string priceListKey = await GetFirstPriceListAsync();

            try
            {
                SalesItemPriceListLineResource newPriceListLine = new SalesItemPriceListLineResource
                {
                    PriceList = priceListKey,
                    Unit = "KM",
                    Price = new MoneyResource()
                    {
                        Value = 123,
                        Currency = "€"
                    }
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = string.Format("salesCore/salesItems/{0}/priceListLines", itemKey);
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postSalesItemMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string salesItemContent = JsonConvert.SerializeObject(newPriceListLine, settings);

                    postSalesItemMessage.Content = new StringContent(salesItemContent, Encoding.UTF8, "application/json");

                    // Send

                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", salesItemContent);

                    using (HttpResponseMessage responseContent = await client.SendAsync(postSalesItemMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception(string.Format("Unable to create a new price line on salesItem '{0}", itemKey));
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("SalesItem Line (ID {0}) created on SalesItem '{1}'", result, itemKey));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error creating the new price list line on salesItem '{0}'", itemKey));
            }
        }

        /// <summary>
        /// Sets the new price asynchronous.
        /// </summary>
        public static async Task SetNewPriceAsync(string itemKey)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(itemKey))
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("SetNewPriceAsync: 'itemKey' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }

            SalesItemPriceListLineResource lineResource = await GetItemFirstPriceLineAsync(itemKey);
            if (lineResource == null)
            {
                // Simply inset new line
                await InsertPriceOnSalesItemAsync(itemKey);
                return;
            }

            try
            {
                double newPrice = lineResource.Price.Value + 10D;

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                // Build the request
                    string request = string.Format("salesCore/salesItems/{0}/priceListLines/{1}/priceAmount", itemKey, lineResource.Id);
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a PUT

                    HttpRequestMessage putSalesItemMessage = new HttpRequestMessage(HttpMethod.Put, resourceLocation);

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string salesItemContent = JsonConvert.SerializeObject(newPrice, settings);

                    putSalesItemMessage.Content = new StringContent(salesItemContent, Encoding.UTF8, "application/json");

                    // Send

                    Console.WriteLine("Request - PUT");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", salesItemContent);

                    using (HttpResponseMessage responseContent = await client.SendAsync(putSalesItemMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception(string.Format("Unable to update the price in the first price list line of the SalesItem '{0}'", itemKey));
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Update first price list line price on SalesItem: {0}", itemKey));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error updating the price in the first price list line of the SalesItem '{0}'", itemKey));
            }
        }

        /// <summary>
        /// Deletes the salesItem extension asynchronous.
        /// </summary>
        /// <param name="salesItem">The salesItem.</param>
        public static async Task DeleteSalesItemExtensionAsync(string itemKey)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(itemKey))
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("DeleteSalesItemExtensionAsync: 'itemKey' parameter is required");
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

                    string request = string.Format("salesCore/salesItems/{0}", itemKey);
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a DELETE

                    HttpRequestMessage deleteSalesItemMessage = new HttpRequestMessage(HttpMethod.Delete, resourceLocation);

                    // Log Request

                    Console.WriteLine("Request - DEL");
                    Console.WriteLine("{0}", resourceLocation);

                    // Send

                    using (HttpResponseMessage responseContent = await client.SendAsync(deleteSalesItemMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception(string.Format("Unable to delete the salesItem extension for Party '{0}", itemKey));
                        }

                        // Succeeded

                        ConsoleHelper.WriteSuccessLine(string.Format("SalesItem Extension for Party '{0}' was deleted", itemKey));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error deleting the salesItem extension for Party '{0}'", itemKey));
            }
        }

        /// <summary>
        /// Creates the sales item extension to an existing asynchronous.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        public static async Task CreateSalesItemExtensionAsync(string itemKey)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(itemKey))
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("CreateSalesItemExtensionAsync: 'itemKey' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }

            string itemTaxSchemaKey = await GetDefaultItemTaxSchemaAsync();

            try
            {
                // Build the customer that will be created

                SalesItemResource newSalesItem = new SalesItemResource()
                {
                    IncomeAccount = "71111",
                    BaseEntityKey = itemKey,
                    Unit = "KM",
                    ItemTaxSchema = itemTaxSchemaKey
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salesCore/salesItems/extension";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postSalesItemMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    // Build the json data

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string salesItemContent = JsonConvert.SerializeObject(newSalesItem, settings);
                    postSalesItemMessage.Content = new StringContent(salesItemContent, Encoding.UTF8, "application/json");

                    // Log Request

                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", salesItemContent);

                    // Send

                    using (HttpResponseMessage responseContent = await client.SendAsync(postSalesItemMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception(string.Format("Unable to create the sales item extention for Item '{0}'.", itemKey));
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("SalesItem extension created for Item '{0}': (Id = {1})", itemKey, result));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error creating the sales item extension for Item '{0}'.", itemKey));
            }
        }

        /// <summary>
        /// Deletes all items created on this sample.
        /// </summary>
        public static async Task DeleteAllItemsCreatedOnSample()
        {
            await DeleteAllSalesItemsCreatedOnSample();
            await DeleteAllBaseSalesItemsCreatedOnSample();
        }

        #endregion

        #region Private Methods

        private static async Task<string> GetDefaultItemTaxSchemaAsync()
        {
            string foundItemTaxSchema = null;
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "taxesCore/itemTaxSchemas/odata";

                    // build the odata expression

                    string odataExpression = "?$select=TaxCodeItemGroupKey&$top=1&$filter=(TaxCodeItemGroupKey eq 'IVA-TN' or TaxCodeItemGroupKey eq 'NORMAL') and IsActive eq true and IsDeleted eq false";

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

                            throw new Exception("Unable to get the default ItemTaxSchema.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<ItemTaxSchemaResource>>(json);
                        IList<ItemTaxSchemaResource> itemTaxSchemas = objectResult.Items;

                        foundItemTaxSchema = itemTaxSchemas[0].ItemTaxSchema;
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error getting the default ItemTaxSchema.");
            }

            return foundItemTaxSchema;
        }

        private static async Task<string> GetFirstPriceListAsync()
        {
            string foundPriceListKey = null;
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salescore/pricelists/odata";

                    // build the odata expression

                    string odataExpression = "?$select=PriceListKey&$top=1&$filter= IsActive eq true and IsSystem eq false and IsDeleted eq false&$orderby=PosInSalesItemsList";

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

                            throw new Exception("Unable to get the first sales price list.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<PriceListResource>>(json);
                        IList<PriceListResource> priceLists = objectResult.Items;

                        foundPriceListKey = priceLists[0].PriceListKey;
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error getting the first sales price list.");
            }

            return foundPriceListKey;
        }

        private static async Task<SalesItemPriceListLineResource> GetItemFirstPriceLineAsync(string itemKey)
        {
            SalesItemPriceListLineResource foundPriceListLine = null;

            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salescore/salesItems/odata";

                    // build the odata expression

                    string odataExpression = string.Format("?$top=1&$filter=ItemKey eq '{0}' and IsActive eq true and IsSystem eq false and IsDeleted eq false", itemKey);

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

                            throw new Exception("Unable to get the first sales price list.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<SalesItemResource>>(json);
                        IList<SalesItemResource> salesItems = objectResult.Items;

                        foundPriceListLine = salesItems[0].PriceListLines.FirstOrDefault();
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error getting the first sales price list.");
            }

            return foundPriceListLine;
        }

        private static async Task DeleteAllSalesItemsCreatedOnSample()
        {
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salescore/salesItems/odata";

                    // build the odata expression

                    string partyKey = $"NewS{DateTime.Now.ToString("yyyyMMddHHmmss")}".Substring(0, 8);
                    string otherPartyKey = $"NewI{DateTime.Now.ToString("yyyyMMddHHmmss")}".Substring(0, 8);

                    string odataExpression = string.Format("?$select=ItemKey&$filter= IsActive eq true and IsSystem eq false and IsDeleted eq false and (startswith(ItemKey, '{0}') or startswith(ItemKey, '{1}')) eq true and startswith(CreatedBy, 'application-user::')", partyKey, otherPartyKey);

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Log Request

                    Console.WriteLine("");
                    Console.WriteLine("Get All Sales Items to delete");
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

                            throw new Exception("Unable to get the sales items to delete.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<SalesItemResource>>(json);
                        IList<SalesItemResource> salesItems = objectResult.Items;

                        // Log 

                        if (salesItems.Count == 0)
                        {
                            Console.WriteLine("");
                            ConsoleHelper.WriteSuccessLine("No sales items found to delete");
                            Console.WriteLine("");
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Delete the salesItems found");
                            Console.WriteLine("");

                            foreach (SalesItemResource foundSalesItem in salesItems)
                            {
                                string foundItemKey = foundSalesItem.ItemKey;

                                // Build the request

                                request = string.Format("salesCore/salesItems/{0}", foundItemKey);
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

                                        throw new Exception(string.Format("Unable to delete the sales items extension for Item '{0}'.", foundItemKey));
                                    }

                                    // Succeeded

                                    ConsoleHelper.WriteSuccessLine(string.Format("SalesItems extension for Item '{0}' was deleted", foundItemKey));
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
                throw new Exception("Error deleting the sales items created by this sample.");
            }
        }

        private static async Task DeleteAllBaseSalesItemsCreatedOnSample()
        {
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "businessCore/items/odata";

                    // build the odata expression

                    string partyKey = $"NewS{DateTime.Now.ToString("yyyyMMddHHmmss")}".Substring(0, 8);
                    string otherPartyKey = $"NewI{DateTime.Now.ToString("yyyyMMddHHmmss")}".Substring(0, 8);

                    string odataExpression = string.Format("?$select=ItemKey&$filter= IsActive eq true and IsSystem eq false and IsDeleted eq false and (startswith(ItemKey, '{0}') or startswith(ItemKey, '{1}')) eq true and startswith(CreatedBy, 'application-user::')", partyKey, otherPartyKey);

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Log Request

                    Console.WriteLine("");
                    Console.WriteLine("Get All Items to delete");
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

                            throw new Exception("Unable to get the items cretaed by this sample.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<ItemResource>>(json);
                        IList<ItemResource> items = objectResult.Items;

                        // Log

                        if (items.Count == 0)
                        {
                            Console.WriteLine("");
                            ConsoleHelper.WriteSuccessLine("No items found to delete");
                            Console.WriteLine("");
                        }
                        else
                        {

                            Console.WriteLine("");
                            Console.WriteLine("Delete the sales items found");
                            Console.WriteLine("");

                            foreach (ItemResource foundItem in items)
                            {
                                string foundItemKey = foundItem.ItemKey;

                                // Build the request

                                request = string.Format("businessCore/items/{0}", foundItemKey);
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

                                        ConsoleHelper.WriteErrorLine(string.Format("Unable to delete the Item '{0}'.", foundItemKey));
                                    }
                                    else
                                    {
                                        // Succeeded

                                        ConsoleHelper.WriteSuccessLine(string.Format("Item '{0}' was deleted", foundItemKey));
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
                throw new Exception("Error deleting all items created by this sample.");
            }
        }

        #endregion
    }
}
