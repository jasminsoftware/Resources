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
        /// Creates a new order.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object that represents the asynchronous task.
        /// </returns>
        public static async Task CreateOrderAsync()
        {
            // Build the order that should be created

            OrderResource order = new OrderResource();

            try
            {
                order.Company = CompanyKey;
                order.DocumentType = "ECL";
                order.DocumentDate = DateTime.UtcNow;
                order.Serie = DateTime.UtcNow.Year.ToString();
                order.CustomerId = "0002";
                order.Currency = "EUR";

                order.Lines = new List<OrderLineResource>
                {
                    new OrderLineResource
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
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "sales/orders";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postOrderMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    string orderRequest = JsonConvert.SerializeObject(order);

                    postOrderMessage.Content = new StringContent(orderRequest, Encoding.UTF8, "application/json");

                    // Send
                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", orderRequest);

                    using (HttpResponseMessage responseContent = await client.SendAsync(postOrderMessage))
                    {
                        // Get the response

                        if (responseContent.IsSuccessStatusCode)
                        {
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteSuccessLine(string.Format("Order created: (Id = {0})", result));
                        }
                        else
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

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

                        if (responseContent.IsSuccessStatusCode)
                        {
                            string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                            JsonSerializerSettings settings = new JsonSerializerSettings()
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                Formatting = Formatting.Indented
                            };

                            var objectResult = JsonConvert.DeserializeObject<ListResponse<OrderResource>>(json);
                            IList<OrderResource> orders = objectResult.Data;

                            ConsoleHelper.WriteSuccessLine("The orders were obtained with success.");
                            Console.WriteLine("");
                            foreach (OrderResource order in orders)
                            {
                                Console.WriteLine("Order: Company {0} - {1}.{2}.{3} (id = {4})", order.Company, order.DocumentType, order.Serie, order.SeriesNumber, order.Id);
                            }

                            Console.WriteLine("");
                        }
                        else
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to list the orders.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error creating the order.");
            }
        }

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

                    string odataExpression = string.Format("?$select=Company, DocumentType, Serie, SeriesNumber, Id&$top=5&$filter= Company eq '{0}' and BuyerCustomerParty eq '{1}'&$orderby=CreatedOn desc", CompanyKey, "0002");

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

                        if (responseContent.IsSuccessStatusCode)
                        {
                            string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                            JsonSerializerSettings settings = new JsonSerializerSettings()
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                Formatting = Formatting.Indented
                            };

                            var objectResult = JsonConvert.DeserializeObject<ODataResponse<OrderResource>>(json);
                            IList<OrderResource> orders = objectResult.Items;

                            ConsoleHelper.WriteSuccessLine(string.Format("The top 5 orders for customer '0002' on company '{0}' were obtained with success.", CompanyKey));
                            Console.WriteLine("");
                            foreach (OrderResource order in orders)
                            {
                                Console.WriteLine("Order: Company {0} - {1}.{2}.{3} (id = {4})", order.Company, order.DocumentType, order.Serie, order.SeriesNumber, order.Id);
                            }

                            Console.WriteLine("");
                        }
                        else
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to list the orders.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error creating the order.");
            }
        }

        public static async Task<OrderResource> GetLastOrderAsync(bool silentMode)
        {
            OrderResource foundOrder = null;
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "sales/orders/odata";

                    // build the odata expression

                    string odataExpression = string.Format("?$top=1&$filter= Company eq '{0}' and BuyerCustomerParty eq '{1}'&$orderby=CreatedOn desc", CompanyKey, "0002");

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

                            JsonSerializerSettings settings = new JsonSerializerSettings()
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                Formatting = Formatting.Indented
                            };

                            var objectResult = JsonConvert.DeserializeObject<ODataResponse<OrderResource>>(json);
                            foundOrder = objectResult.Items.FirstOrDefault();

                            if (!silentMode)
                            {
                                ConsoleHelper.WriteSuccessLine(string.Format("The last order of customer '0002' on company '{0}' was found with success.", CompanyKey));
                                Console.WriteLine("");
                            }

                            if (foundOrder != null && !silentMode)
                            {
                                Console.WriteLine("Order: Company {0} - {1}.{2}.{3} (id = {4})", foundOrder.Company, foundOrder.DocumentType, foundOrder.Serie, foundOrder.SeriesNumber, foundOrder.Id);
                                Console.WriteLine("");
                                foreach (OrderLineResource line in foundOrder.Lines.OrderBy(l => l.Index))
                                {
                                    Console.WriteLine("Order Line: (Id = {0}) - '{1}' '{2}' QTD = {3}", line.Id, line.ItemId, line.Description, line.Quantity);
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

                                throw new Exception("Unable to get the last order.");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error getting the last order.");
            }

            return foundOrder;
        }

        public static async Task InsertLineOnOrderAsync(OrderResource order)
        {
            try
            {
                OrderLineResource orderLine = new OrderLineResource
                {
                    ItemId = "0001",
                    Quantity = 10,
                    Type = 1,

                    Price = new Price()
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

                    string orderRequest = JsonConvert.SerializeObject(orderLine);

                    postOrderMessage.Content = new StringContent(orderRequest, Encoding.UTF8, "application/json");

                    // Send

                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", orderRequest);

                    using (HttpResponseMessage responseContent = await client.SendAsync(postOrderMessage))
                    {
                        // Get the response

                        if (responseContent.IsSuccessStatusCode)
                        {
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteSuccessLine(string.Format("Order Line (ID {0}) created on Order: Company {1} - {2}.{3}.{4})", result, order.Company, order.DocumentType, order.Serie, order.SeriesNumber));
                        }
                        else
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to create a new line on order.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error creating the new line on order.");
            }
        }

        public static async Task SetQuantityOnOrderLastLineAsync(OrderResource order)
        {
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

                        string orderRequest = JsonConvert.SerializeObject(quantity);

                        putOrderMessage.Content = new StringContent(orderRequest, Encoding.UTF8, "application/json");

                        // Send

                        Console.WriteLine("Request - PUT");
                        Console.WriteLine("{0}", resourceLocation);
                        Console.WriteLine("Request - BODY ");
                        Console.WriteLine("{0}", orderRequest);

                        using (HttpResponseMessage responseContent = await client.SendAsync(putOrderMessage))
                        {
                            // Get the response

                            if (responseContent.IsSuccessStatusCode)
                            {
                                string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                                ConsoleHelper.WriteSuccessLine(string.Format("Order Line Updated quantity on Order: Company {0} - {1}.{2}.{3})", order.Company, order.DocumentType, order.Serie, order.SeriesNumber));
                            }
                            else
                            {
                                ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                                string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                                ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                                throw new Exception("Unable to update line quantity on order.");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Error creating the new line on order.");
                }
            }
        }

        public static async Task DeleteLastLineOnOrderAsync(OrderResource order)
        {
            try
            {
                if (order.Lines.Count == 1)
                {
                    ConsoleHelper.WriteErrorLine("Order has only one line we cannot delete it");
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

                        if (responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteSuccessLine(string.Format("Order Line (ID {0}) deleted on Order: Company {1} - {2}.{3}.{4})", line.Id, order.Company, order.DocumentType, order.Serie, order.SeriesNumber));
                        }
                        else
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to delete the last line on order.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error deleting the last line on order.");
            }
        }

        public static async Task DeleteLastOrderAsync(OrderResource order)
        {
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

                        if (responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteSuccessLine(string.Format("Order was deleted: Company {0} - {1}.{2}.{3})", order.Company, order.DocumentType, order.Serie, order.SeriesNumber));
                        }
                        else
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to delete the last order.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error deleting the last order.");
            }
        }
        #endregion
    }
}