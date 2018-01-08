using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ListSalesItem;
using System.Web.Http.OData;
using System.Linq;
using System.Net.Http.Formatting;

namespace UpdateSalesItem
{
    public static class Program
    {
        #region Constants

        private const string ApiBaseAddress = "https://my.jasminsoftware.com/api";

        private const string AccountKey = "10001"; // TODO: put your account key here
        private const string SubscriptionKey = "10001-0002"; // TODO: put your account key here

        private const string AccessToken = "ew0KICAidHlwIjogIkpXVCIsDQogICJhbGciOiAiUlMyNTYiLA0KICAieDV0IjogImEzck1VZ01Gdjl0UGNsTGE2eUYzekFrZnF1RSIsDQogICJraWQiOiAiYTNyTVVnTUZ2OXRQY2xMYTZ5RjN6QWtmcXVFIg0KfQ.ew0KICAiaXNzIjogImh0dHBzOi8vaWRlbnRpdHkucHJpbWF2ZXJhYnNzLmNvbS9jb3JlIiwNCiAgImF1ZCI6ICJodHRwczovL2lkZW50aXR5LnByaW1hdmVyYWJzcy5jb20vY29yZS9yZXNvdXJjZXMiLA0KICAiZXhwIjogMTUxNTQ0MjYyMiwNCiAgIm5iZiI6IDE1MTU0MzE4MjIsDQogICJjbGllbnRfaWQiOiAiamFzbWluIiwNCiAgInNjb3BlIjogWw0KICAgICJhcHBsaWNhdGlvbiIsDQogICAgIm9wZW5pZCIsDQogICAgIm12YyINCiAgXSwNCiAgInN1YiI6ICI4YzgzYzRkNC0wYzI4LTQ3OGItOGQ3Yy05NzBhN2Y4N2JhMWIiLA0KICAiYXV0aF90aW1lIjogMTUxNTQzMTgyMiwNCiAgImlkcCI6ICJpZHNydiIsDQogICJwcmVmZXJyZWRfdXNlcm5hbWUiOiAic29maWEucmliZWlyb0BwcmltYXZlcmFic3MuY29tIiwNCiAgImVtYWlsIjogInNvZmlhLnJpYmVpcm9AcHJpbWF2ZXJhYnNzLmNvbSIsDQogICJlbWFpbF92ZXJpZmllZCI6ICJ0cnVlIiwNCiAgIm5hbWUiOiAic29maWEucmliZWlyb0BwcmltYXZlcmFic3MuY29tIiwNCiAgImdpdmVuX25hbWUiOiAiYWRtaW4iLA0KICAiZmFtaWx5X25hbWUiOiAiIiwNCiAgInBpY3R1cmUiOiAiIiwNCiAgImN1bHR1cmUiOiAiZW4tVVMiLA0KICAidXNlcm5hbWUiOiAic29maWEucmliZWlyb0BwcmltYXZlcmFic3MuY29tIiwNCiAgImRpc3BsYXlfbmFtZSI6ICJhZG1pbiIsDQogICJqdGkiOiAiMzMxNWJiNWNkMWZiNTIwNmY1MGQ3NmI2ODQzMzI1OTgiLA0KICAiYW1yIjogWw0KICAgICJwYXNzd29yZCINCiAgXQ0KfQ.UqqMexjhLTRzDx7RevQp8KdeFXgABMcx7XBYcUrduW__gAtT0t7_xaf_9o2uglxWvaQoUKbdTrPOVQZEDnVxnp1LB-x358KjUyLewH46aq_BHwM_ULaClZyiGP5oEGBlXJNHmfpxxezPCAxJlPS9GuA_nvIo-eQlJZ4jn_joZV6eFUwYl2RKwi8aPWsKBPA0_gQEzMdDTuv6bddyTcK5xuihoBLWlUhOpKTnr9h_enR842R7wrKFVRNSjFifxfFiAYu8fdIZqeDhSFWu6ME7ypEtyDcmEvV7T5W9lUQWMCI1tLXWFRUMG6poqkl32rewK-Sd49LVlgUV-Yk6LaXwXA"; // TODO: put the authorization access token here (this should be obtained previously)
        private const string ItemKey = "P3037"; //TODO: Put the Item Key here

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            // Start

            Console.WriteLine("Jasmin rocks! Let's update the Price List Amount...");

            // Handle errors

            try
            {
                // Create the HTTP client to send the request

                using (HttpClient client = new HttpClient())
                {
                    // Set the authorization header

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                    // Get the Sales Item to update the Price List PVP2 price amount.
                    SalesItemResource salesItem = await GetSalesItemResourceAsync(client, ItemKey);

                    // Check if PVP2 price list exists
                    var priceListLine = salesItem.PriceListLines.FirstOrDefault(item => item.PriceList.Equals("PVP2", StringComparison.OrdinalIgnoreCase));
                    if (priceListLine == null)
                    {
                        throw new Exception("Price List PVP2 does not exsits.");
                    }

                    Console.WriteLine("Price List Line - PVP2: {0} {1} {2}", priceListLine.Id, priceListLine.PriceList, priceListLine.PriceAmount.Value);

                    // Get a random number and update the price list line amount value
                    decimal value = GetRandomNumber();
                    await UpdatePriceListLinePriceAmountAsync(client, salesItem.Id, priceListLine.Id, value);

                    // Let's validate if the value was updated...

                    // Get the Sales Item with the updated Price List PVP2 price amount.
                    salesItem = await GetSalesItemResourceAsync(client, ItemKey);

                    // Check if PVP2 price list has the updated value
                    priceListLine = salesItem.PriceListLines.FirstOrDefault(item => item.PriceList.Equals("PVP2", StringComparison.OrdinalIgnoreCase));
                    if (priceListLine != null)
                    {
                        Console.WriteLine("Price List Line - PVP2: {0} {1} {2}", priceListLine.Id, priceListLine.PriceList, priceListLine.PriceAmount.Value);
                    }
                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
            }

            // End

            Console.Write("Press any key to end... ");
            Console.ReadKey();            
        }

        private static async Task<SalesItemResource> GetSalesItemResourceAsync(HttpClient client, string itemKey)
        {
            // List Sales Items by Key to get the LineID attribute
            // URI of the endpoint

            string endpoint = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}/salesCore/salesItems/{3}/extension", ApiBaseAddress, AccountKey, SubscriptionKey, itemKey);

            // Send the request (GET)

            Console.WriteLine("List the sales item by key...");

            Uri endpointUri = new Uri(endpoint);

            using (HttpResponseMessage response = await client.GetAsync(endpointUri))
            {
                // Failed?

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "The requested failed with status code {0} ({1}).", (int)response.StatusCode, response.StatusCode));

                    if (!string.IsNullOrWhiteSpace(errorContent))
                    {
                        sb.Append(string.Format(CultureInfo.CurrentCulture, "Message: {0}.", errorContent));
                    }

                    throw new InvalidOperationException(sb.ToString());
                }

                // Succeeded

                string json = await response.Content.ReadAsStringAsync();

                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                };

                SalesItemResource salesItem = JsonConvert.DeserializeObject<SalesItemResource>(json);

                Console.WriteLine("The Sales item attributes were obtained with success.");
                Console.WriteLine("Sales Item: {0}.{1}", salesItem.Id, salesItem.Itemkey);

                return salesItem;
            }
        }

        private static async Task UpdatePriceListLinePriceAmountAsync(HttpClient client, Guid salesItemId, string priceListLineId, decimal newAmount)
        {
            try
            {
                // URI of the endpoint

                string endpoint = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}/salesCore/salesItems/{3}/priceListLines/{4}/priceAmount", ApiBaseAddress, AccountKey, SubscriptionKey, ItemKey, priceListLineId);

                // Update the Price List Amount

                Uri endpointUri = new Uri(endpoint);

                using (HttpResponseMessage response = await client.PutAsync<decimal>(endpointUri, newAmount, new JsonMediaTypeFormatter()))
                {
                    // Failed?

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "The requested failed with status code {0} ({1}).", (int)response.StatusCode, response.StatusCode));

                        if (!string.IsNullOrWhiteSpace(errorContent))
                        {
                            sb.Append(string.Format(CultureInfo.CurrentCulture, "Message: {0}.", errorContent));
                        }

                        throw new InvalidOperationException(sb.ToString());
                    }

                    // Succeeded

                    Console.WriteLine("The Amount was updated with sucess...");

                }
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteErrorLine("Error found!");
                ConsoleHelper.WriteErrorLine(exception.Message);
            }
        }
        
        #endregion

        #region Helper Methods

        private static decimal GetRandomNumber()
        {
            // Get a random number
            var rand = new Random();
            var value = new decimal(rand.NextDouble());
            return value;
        }

        #endregion
    }
}

