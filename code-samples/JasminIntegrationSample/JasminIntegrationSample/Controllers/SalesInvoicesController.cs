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
    /// Defines a controller capable of managing sales invoices.
    /// </summary>
    internal static class SalesInvoicesController
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
        /// Creates a new invoice asyncromou.
        /// </summary>
        /// <returns>A task.</returns>
        public static async Task CreateInvoiceAsync()
        {
            try
            {
                // Build the invoice to be created

                SalesInvoiceResource invoice = new SalesInvoiceResource()
                {
                    Company = CompanyKey,
                    DocumentType = "FS",
                    DocumentDate = DateTime.UtcNow,
                    Serie = DateTime.UtcNow.Year.ToString(),
                    Customer = "SOFRIO",
                    Currency = "EUR",
                };

                invoice.Lines = new List<SalesInvoiceLineResource>
                {
                    new SalesInvoiceLineResource
                    {
                        Item = "ARECA",
                        Warehouse = "01",
                        Quantity = 1,
                        Description = "Invoice created by Sample",
                        Price = new MoneyResource {
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

                    string request = "billing/invoices";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a POST

                    HttpRequestMessage postInvoiceMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    }; 

                    string invoiceContent = JsonConvert.SerializeObject(invoice, settings);

                    postInvoiceMessage.Content = new StringContent(invoiceContent, Encoding.UTF8, "application/json");

                    // Log Request

                    Console.WriteLine("Request - POST");
                    Console.WriteLine("{0}", resourceLocation);
                    Console.WriteLine("Request - BODY ");
                    Console.WriteLine("{0}", invoiceContent);

                    // Send

                    using (HttpResponseMessage responseContent = await client.SendAsync(postInvoiceMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string errorResult = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", errorResult));

                            throw new Exception("Unable to create the invoice.");
                        }

                        // Succeeded

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                        ConsoleHelper.WriteSuccessLine(string.Format("Invoice created: (Id = {0})", result));
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error creating the invoice.");
            }
        }

        /// <summary>
        /// Gets the first 20 invoices asynchronous.
        /// </summary>
        public static async Task GetInvoicesAsync()
        {
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "billing/invoices?page=1&pageSize=20";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Log Request

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

                            throw new Exception("Unable to list the invoices.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ListResponse<SalesInvoiceResource>>(json);
                        IList<SalesInvoiceResource> invoices = objectResult.Data;

                        ConsoleHelper.WriteSuccessLine("The invoices were obtained with success.");
                        Console.WriteLine("");
                        foreach (SalesInvoiceResource invoice in invoices)
                        {
                            Console.WriteLine("Invoice: Company {0} - {1}.{2}.{3} (id = {4})", invoice.Company, invoice.DocumentType, invoice.Serie, invoice.SeriesNumber, invoice.InvoiceId);
                        }

                        Console.WriteLine("");
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error creating the invoice.");
            }
        }

        /// <summary>
        /// Gets the top5 invoices asynchronous.
        /// </summary>
        public static async Task GetTop5InvoicesAsync()
        {
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "billing/invoices/odata";

                    // build the odata expression

                    string odataExpression = string.Format("?$inlinecount=allpages&$select=Company, DocumentType, Serie, SeriesNumber, Id&$top=5&$filter= Company eq '{0}' and BuyerCustomerParty eq '{1}'&$orderby=CreatedOn desc", CompanyKey, "SOFRIO");

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Log Request

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

                            throw new Exception("Unable to list the invoices.");
                        }

                        // Succeeded

                        string json = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        var objectResult = JsonConvert.DeserializeObject<ODataResponse<SalesInvoiceResource>>(json);
                        IList<SalesInvoiceResource> invoices = objectResult.Items;

                        ConsoleHelper.WriteSuccessLine(string.Format("The top 5 invoices for customer '{0}' on company '{1}' were obtained with success.", "SOFRIO", CompanyKey));
                        Console.WriteLine("");
                        foreach (SalesInvoiceResource invoice in invoices)
                        {
                            Console.WriteLine("Invoice: Company {0} - {1}.{2}.{3} (id = {4})", invoice.Company, invoice.DocumentType, invoice.Serie, invoice.SeriesNumber, invoice.InvoiceId);
                        }

                        Console.WriteLine("Invoices Count: {0}", invoices.Count);
                        Console.WriteLine("");
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error creating the invoice.");
            }
        }

        /// <summary>
        /// Gets the last invoice asynchronous.
        /// </summary>
        /// <param name="silentMode">if set to <c>true</c> console messages will not be displayed.</param>
        /// <returns></returns>
        public static async Task<SalesInvoiceResource> GetLastInvoiceAsync(bool silentMode = false)
        {
            SalesInvoiceResource foundInvoice = null;
            try
            {
                // Create the HTTP client to perform the request

                using (HttpClient client = new HttpClient())
                {
                    await AuthenticationProvider.SetAccessTokenAsync(client);

                    // Build the request

                    string request = "billing/invoices/odata";

                    // build the odata expression

                    string odataExpression = string.Format("?$top=1&$filter= Company eq '{0}' and BuyerCustomerParty eq '{1}'&$orderby=CreatedOn desc", CompanyKey, "SOFRIO");

                    // full request 
                    request = string.Concat(request, odataExpression);

                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // Log Request

                    if (!silentMode)
                    {
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

                            var objectResult = JsonConvert.DeserializeObject<ODataResponse<SalesInvoiceResource>>(json);
                            foundInvoice = objectResult.Items.FirstOrDefault();

                            if (!silentMode)
                            {
                                ConsoleHelper.WriteSuccessLine(string.Format("The last invoice of customer '{0}' on company '{1}' was found with success.", "SOFRIO", CompanyKey));
                                Console.WriteLine("");
                            }

                            if (foundInvoice != null && !silentMode)
                            {
                                Console.WriteLine("Invoice: Company {0} - {1}.{2}.{3} (id = {4})", foundInvoice.Company, foundInvoice.DocumentType, foundInvoice.Serie, foundInvoice.SeriesNumber, foundInvoice.InvoiceId);
                                Console.WriteLine("");
                                foreach (SalesInvoiceLineResource line in foundInvoice.Lines.OrderBy(l => l.Index))
                                {
                                    Console.WriteLine("Invoice Line: (Id = {0}) - '{1}' '{2}' QTD = {3}", line.Id, line.Item, line.Description, line.Quantity);
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

                                throw new Exception("Unable to get the last invoice.");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error getting the last invoice.");
            }

            return foundInvoice;
        }

        /// <summary>
        /// Deletes the invoice asynchronous.
        /// </summary>
        /// <param name="invoice">The invoice to delete.</param>
        public static async Task DeleteInvoiceAsync(SalesInvoiceResource invoice)
        {
            // Validate Parameters

            if (invoice == null)
            {
                ConsoleHelper.WriteErrorLine("");
                ConsoleHelper.WriteErrorLine("DeleteInvoiceAsync: 'invoice' parameter is required");
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

                    string request = "billing/invoices";
                    string resourceLocation = string.Format("{0}/api/{1}/{2}/{3}/{4}/{5}/{6}/{7}", Constants.baseAppUrl, AccountKey, SubscriptionKey, request, invoice.Company, invoice.DocumentType, invoice.Serie, invoice.SeriesNumber);

                    client.SetDefaultRequestHeaders(CultureKey);

                    // It's a DELETE

                    HttpRequestMessage deleteInvoiceLineMessage = new HttpRequestMessage(HttpMethod.Delete, resourceLocation);

                    // Send

                    Console.WriteLine("Request - DEL");
                    Console.WriteLine("{0}", resourceLocation);

                    using (HttpResponseMessage responseContent = await client.SendAsync(deleteInvoiceLineMessage))
                    {
                        // Get the response

                        if (!responseContent.IsSuccessStatusCode)
                        {
                            ConsoleHelper.WriteErrorLine(string.Format("Failed. {0}", responseContent.ToString()));
                            string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();
                            ConsoleHelper.WriteErrorLine(string.Format("Content: {0}", result));

                            throw new Exception("Unable to delete the last invoice.");
                        }

                        // Succeeded

                        ConsoleHelper.WriteSuccessLine(string.Format("Invoice was deleted: Company {0} - {1}.{2}.{3})", invoice.Company, invoice.DocumentType, invoice.Serie, invoice.SeriesNumber));

                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
                throw new Exception("Error deleting the last invoice.");
            }
        }

        #endregion
    }
}