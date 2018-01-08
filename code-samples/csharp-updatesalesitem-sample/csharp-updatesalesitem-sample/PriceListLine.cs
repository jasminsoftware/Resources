namespace UpdateSalesItem
{
    internal class PriceListLineResource
    {
        public string Id { get; set; }

        public string PriceList { get; set; }

        public MoneyResource PriceAmount { get; set; }
    }
}