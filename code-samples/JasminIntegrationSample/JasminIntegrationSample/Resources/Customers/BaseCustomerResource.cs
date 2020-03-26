namespace Jasmin.IntegrationSample
{
    internal class BaseCustomerResource
    {
        #region Internal Properties

        public string CustomerGroup { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentTerm { get; set; }

        public string PartyTaxSchema { get; set; }

        public bool Locked { get; set; }

        public bool OneTimeCustomer { get; set; }

        public bool EndCustomer { get; set; }

        public string PartyKey { get; set; }

        public string SearchTerm { get; set; }

        public string Name { get; set; }

        public string CityName { get; set; }

        public string PostalZone { get; set; }

        public string Telephone { get; set; }

        public string StreetName { get; set; }

        public string BuildingNumber { get; set; }

        #endregion
    }
}
