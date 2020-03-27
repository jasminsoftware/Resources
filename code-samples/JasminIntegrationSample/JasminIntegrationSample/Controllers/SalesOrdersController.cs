using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jasmin.IntegrationSample.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jasmin.IntegrationSample
{
    /// <summary>
    /// Defines a controller capable of managing sales orders.
    /// </summary>
    internal static class SalesOrdersController
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
        /// Gets or sets the company key.
        /// </summary>
        public static string CompanyKey { get; set; }

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
        /// Creates the order asynchronous.
        /// </summary>
        public static async Task CreateOrderAsync()
        {
            string orderTypeKey = await GetOrderTypeWithoutFiscalDocumentTypeAsync();

            try
            {
                // Build the order to be created

                SalesOrderResource newOrder = new SalesOrderResource()
                {
                    Company = CompanyKey,
                    DocumentType = orderTypeKey,
                    DocumentDate = DateTime.UtcNow,
                    Serie = DateTime.UtcNow.Year.ToString(),
                    Customer = "SOFRIO",
                    Currency = "EUR",
                };

                newOrder.Lines = new List<SalesOrderLineResource>
                {
                    new SalesOrderLineResource
                    {
                        Item = "ARECA",
                        Quantity = 1,
                        ItemType = 1,
                        Price = new MoneyResource
                        {
                            Value = 50,
                            Currency = "€"
                        }
                    }
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "sales/orders";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postOrderMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string orderContent = JsonConvert.SerializeObject(newOrder, settings);

                    postOrderMessage.Content = new StringContent(orderContent, Encoding.UTF8, "application/json");

                    // Send
                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", orderContent);

                    using (HttpResponseMessage responseContent = await client.SendAsync(postOrderMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception("Unable to create the sales order.");
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Sales Order created: (Id = {0})", result));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error creating the sales order.");
            }
        }

        /// <summary>
        /// Gets the first 20 orders asynchronous.
        /// </summary>
        public static async Task GetOrdersAsync()
        {
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "sales/orders?page=1&pageSize=20";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Send

                    Console.WriteLine("Request - GET");
                    Console.WriteLine("{0}", resourceLocation);

                    using (HttpResponseMessage responseContent = await client.GetAsync(resourceLocation))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to list the sales orders.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ListResponse<SalesOrderResource>>(json);
                        IList<SalesOrderResource> orders = objectResult.Data;

                        ConsoleHelper.WriteSuccessLine("The sales orders were obtained with success.");
                        Console.WriteLine("");
                        foreach (SalesOrderResource order in orders)
                        {
                            Console.WriteLine("Sales Order: Company {0} - {1}.{2}.{3} (id = {4})", order.Company, order.DocumentType, order.Serie, order.SeriesNumber, order.OrderId);
                        }

                        Console.WriteLine("");
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error creating the order.");
            }
        }

        /// <summary>
        /// Gets the top5 orders asynchronous.
        /// </summary>
        public static async Task GetTop5OrdersAsync()
        {
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "sales/orders/odata";

                    // build the odata expression

                    string odataExpression = string.Format("?$inlinecount=allpages&$select=Company, DocumentType, Serie, SeriesNumber, Id&$top=5&$filter= Company eq '{0}' and BuyerCustomerParty eq '{1}'&$orderby=CreatedOn desc", CompanyKey, "SOFRIO");

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Send

                    Console.WriteLine("Request - GET");
                    Console.WriteLine("{0}", resourceLocation);

                    using (HttpResponseMessage responseContent = await client.GetAsync(resourceLocation))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception("Unable to list the sales orders.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<SalesOrderResource>>(json);
                        IList<SalesOrderResource> orders = objectResult.Items;

                        ConsoleHelper.WriteSuccessLine(string.Format("The top 5 sales orders for customer '{0}' on company '{1}' were obtained with success.", "SOFRIO", CompanyKey));
                        Console.WriteLine("");
                        foreach (SalesOrderResource order in orders)
                        {
                            Console.WriteLine("Sales Order: Company {0} - {1}.{2}.{3} (id = {4})", order.Company, order.DocumentType, order.Serie, order.SeriesNumber, order.OrderId);
                        }

                        Console.WriteLine("Sales Orders Count: {0}", orders.Count);
                        Console.WriteLine("");
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error creating the sales order.");
            }
        }

        /// <summary>
        /// Gets the last order asynchronous.
        /// </summary>
        /// <param name="silentMode">if set to <c>true</c> console messages will not be displayed.</param>
        /// <returns>The SalesOrder Resource.</returns>
        public static async Task<SalesOrderResource> GetLastOrderAsync(bool silentMode = false)
        {
            SalesOrderResource foundOrder = null;
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "sales/orders/odata";

                    // build the odata expression

                    string odataExpression = string.Format("?$top=1&$filter= Company eq '{0}' and BuyerCustomerParty eq '{1}'&$orderby=CreatedOn desc", CompanyKey, "SOFRIO");

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Send

                    if (!silentMode)
                    {
                        Console.WriteLine("Request - GET");
                        Console.WriteLine("{0}", resourceLocation);
                    }

                    using (HttpResponseMessage responseContent = await client.GetAsync(resourceLocation))
                    {
                        // Get the response

                        if (responseContent.IsSuccessStatusCode)
                        {
                            string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                            var objectResult = JsonConvert.DeserializeObject<ODataResponse<SalesOrderResource>>(json);
                            foundOrder = objectResult.Items.FirstOrDefault();

                            if (!silentMode)
                            {
                                ConsoleHelper.WriteSuccessLine(string.Format("The last sales order of customer '{0}' on company '{1}' was found with success.", "SOFRIO", CompanyKey));
                                Console.WriteLine("");
                            }

                            if (foundOrder != null && !silentMode)
                            {
                                Console.WriteLine("Sales Order: Company {0} - {1}.{2}.{3} (id = {4})", foundOrder.Company, foundOrder.DocumentType, foundOrder.Serie, foundOrder.SeriesNumber, foundOrder.OrderId);
                                Console.WriteLine("");
                                foreach (SalesOrderLineResource line in foundOrder.Lines.OrderBy(l => l.Index))
                                {
                                    Console.WriteLine("Order Line: (Id = {0}) - '{1}' '{2}' QTD = {3}", line.Id, line.Item, line.Description, line.Quantity);
                                }

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

                                throw new Exception("Unable to get the last sales order.");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error getting the last sales order.");
            }

            return foundOrder;
        }

        /// <summary>
        /// Inserts the line on order asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        public static async Task InsertLineOnOrderAsync(SalesOrderResource order)
        {
            // Validate Parameters

            if (order == null)
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("InsertLineOnOrderAsync: 'order' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }

            try
            {
                SalesOrderLineResource newOrderLine = new SalesOrderLineResource
                {
                    Item = "BLROSAS",
                    Quantity = 10,
                    ItemType = 1,

                    Price = new MoneyResource()
                    {
                        Value = 20,
                        Currency = "€"
                    }
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "sales/orders";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/{4}/{5}/{6}/{7}/documentLines", Constants.baseAppUrl, AccountKey, SubscriptionKey, request, order.Company, order.DocumentType, order.Serie, order.SeriesNumber);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postOrderMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string orderContent = JsonConvert.SerializeObject(newOrderLine, settings);

                    postOrderMessage.Content = new StringContent(orderContent, Encoding.UTF8, "application/json");

                    // Send

                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", orderContent);

                    using (HttpResponseMessage responseContent = await client.SendAsync(postOrderMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception("Unable to create a new line on sales order.");
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Order Line (ID {0}) created on Sales Order: Company {1} - {2}.{3}.{4})", result, order.Company, order.DocumentType, order.Serie, order.SeriesNumber));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error creating the new line on sales order.");
            }
        }

        /// <summary>
        /// Sets the quantity on order's last line asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        public static async Task SetQuantityOnOrderLastLineAsync(SalesOrderResource order)
        {
            // Validate Parameters

            if (order == null)
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("SetQuantityOnOrderLastLineAsync: 'order' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }

            var line = order.Lines
                .Select(l => new {
                    l.Id,
                    l.Index,
                    l.Quantity
                })
                .OrderByDescending(l => l.Index).FirstOrDefault();

            if (line != null)
            {
                double quantity = line.Quantity + 2D;

                try
                {
                    // Create the HTTP client to perform the request

                    using (HttpClient client = new HttpClient())
                    {
                        await AuthenticationProvider.SetAccessTokenAsync(client);

                        // Build the request
                        string request = "sales/orders";
                        string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/{4}/{5}/{6}/{7}/documentLines/{8}/quantity", Constants.baseAppUrl, AccountKey, SubscriptionKey, request, order.Company, order.DocumentType, order.Serie, order.SeriesNumber, line.Id);

                        client.SetDefaultRequestHeaders(CultureKey);

                        // It's a PUT

                        HttpRequestMessage putOrderMessage = new HttpRequestMessage(HttpMethod.Put, resourceLocation);

                        JsonSerializerSettings settings = new JsonSerializerSettings()
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            Formatting = Formatting.Indented
                        };

                        string orderContent = JsonConvert.SerializeObject(quantity, settings);

                        putOrderMessage.Content = new StringContent(orderContent, Encoding.UTF8, "application/json");

                        // Send

                        Console.WriteLine("Request - PUT");
                        Console.WriteLine("{0}", resourceLocation);
                        Console.WriteLine("Request - BODY ");
                        Console.WriteLine("{0}", orderContent);

                        using (HttpResponseMessage responseContent = await client.SendAsync(putOrderMessage))
                        {
                            // Get the response

                            if (!responseContent.IsSuccessStatusCode)
                            {
                                ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                                string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                                ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                                throw new Exception("Unable to update line quantity on sales order.");
                            }

                            // Succeeded

                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteSuccessLine(string.Format("Order Line Updated quantity on Sales Order: Company {0} - {1}.{2}.{3})", order.Company, order.DocumentType, order.Serie, order.SeriesNumber));
                        }
                    }
                }
                catch (Exception exception)
                {
                    ConsoleHelper.WriteErrorLine("Error found!");
                    ConsoleHelper.WriteErrorLine(exception.Message);
                    throw new Exception("Error creating the new line on sales order.");
                }
            }
        }

        /// <summary>
        /// Deletes the last line on order asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        public static async Task DeleteLastLineOnOrderAsync(SalesOrderResource order)
        {
            // Validate Parameters

            if (order == null)
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("DeleteLastLineOnOrderAsync: 'order' parameter is required");
                ConsoleHelper.WriteErrorLine("");

                return;
            }


            try
            {
                if (order.Lines.Count == 1)
                {
                    ConsoleHelper.WriteErrorLine("Sales Order has only one line we cannot delete it");

                    return;
                }

                var line = order.Lines
                .Select(l => new {
                    l.Id,
                    l.Index
                })
                .OrderByDescending(l => l.Index).FirstOrDefault();

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "sales/orders";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/{4}/{5}/{6}/{7}/documentLines/{8}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request, order.Company, order.DocumentType, order.Serie, order.SeriesNumber, line.Id);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a DELETE

                    HttpRequestMessage deleteOrderLineMessage = new HttpRequestMessage(HttpMethod.Delete, resourceLocation);

                    // Send

                    Console.WriteLine("Request - DEL");
                    Console.WriteLine("{0}", resourceLocation);

                    using (HttpResponseMessage responseContent = await client.SendAsync(deleteOrderLineMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to delete the last line on sales order.");
                        }

                        // Succeeded

                        ConsoleHelper.WriteSuccessLine(string.Format("Order Line (ID {0}) deleted on Sales Order: Company {1} - {2}.{3}.{4})", line.Id, order.Company, order.DocumentType, order.Serie, order.SeriesNumber));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error deleting the last line on sales order.");
            }
        }

        /// <summary>
        /// Deletes the order asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        public static async Task DeleteOrderAsync(SalesOrderResource order)
        {
            // Validate Parameters

            if (order == null)
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("DeleteOrderAsync: 'order' parameter is required");
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

                    string request = "sales/orders";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/{4}/{5}/{6}/{7}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request, order.Company, order.DocumentType, order.Serie, order.SeriesNumber);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a DELETE

                    HttpRequestMessage deleteOrderLineMessage = new HttpRequestMessage(HttpMethod.Delete, resourceLocation);

                    // Send

                    Console.WriteLine("Request - DEL");
                    Console.WriteLine("{0}", resourceLocation);

                    using (HttpResponseMessage responseContent = await client.SendAsync(deleteOrderLineMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to delete the last sales order.");
                        }

                        // Succeeded

                        ConsoleHelper.WriteSuccessLine(string.Format("Sales Order was deleted: Company {0} - {1}.{2}.{3})", order.Company, order.DocumentType, order.Serie, order.SeriesNumber));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error deleting the last sales order.");
            }
        }

        #endregion

        #region Private Methods

        public static async Task<string> GetOrderTypeWithoutFiscalDocumentTypeAsync()
        {
            string orderTypeKey = string.Empty;

            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salescore/ordertypes/odata";

                    // build the odata expression

                    string odataExpression = string.Format("?$select=TypeKey&$top=1&$filter=Company eq '{0}' and OrderNature eq 'StandardOrder' and IsInternal eq false  and IsActive eq true and IsDeleted eq false and FiscalDocumentTypeId eq null and DeliveryOnInvoice eq false&$orderby=CreatedOn desc", CompanyKey);

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Send

                    Console.WriteLine("Request - GET");
                    Console.WriteLine("{0}", resourceLocation);

                    using (HttpResponseMessage responseContent = await client.GetAsync(resourceLocation))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception(string.Format("Unable to get the sales order type on company '{0}'.", CompanyKey));
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<SalesOrderTypeResource>>(json);
                        IList<SalesOrderTypeResource> salesOrderTypes = objectResult.Items;
                        if (salesOrderTypes.Count > 0)
                        {
                            orderTypeKey = salesOrderTypes[0].TypeKey;
                            ConsoleHelper.WriteSuccessLine(string.Format("Sales Order type '{0}' was obtained with success on company {1}.", orderTypeKey, CompanyKey));
                            Console.WriteLine("");
                        }
                        else
                        {
                            orderTypeKey = await CreateOrderTypeWithoutFiscalDocumentTypeAsync();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error getting sales order type.");
            }

            return orderTypeKey;
        }

        public static async Task<string> CreateOrderTypeWithoutFiscalDocumentTypeAsync()
        {
            // the order type Key to be created.

            string orderTypeKey = "ECLTST";

            try
            {
                // Build the order type to be created

                SalesOrderTypeResource newOrderType = new SalesOrderTypeResource()
                {
                    Company = CompanyKey,
                    TypeKey = orderTypeKey,
                    OrderNature = "StandardOrder"
                };

                newOrderType.Lines = new List<SalesOrderTypeLineSerieResource>
                {
                    new SalesOrderTypeLineSerieResource
                    {
                        IsDefault = true,
                        Serie = DateTime.UtcNow.Year.ToString(),
                    }
                };

                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "salesCore/orderTypes";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postOrderTypeMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    };

                    string orderTypeContent = JsonConvert.SerializeObject(newOrderType, settings);

                    postOrderTypeMessage.Content = new StringContent(orderTypeContent, Encoding.UTF8, "application/json");

                    // Send
                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", orderTypeContent);

                    using (HttpResponseMessage responseContent = await client.SendAsync(postOrderTypeMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception(string.Format("Unable to create the sales order type '{0} on company '{1}'.", orderTypeKey, CompanyKey));
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Sales Order type created: (Id = {0})", result));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception(string.Format("Error creating the sales order type '{0} on company '{1}'.", orderTypeKey, CompanyKey));
            }

            return orderTypeKey;
        }

        #endregion
    }
}