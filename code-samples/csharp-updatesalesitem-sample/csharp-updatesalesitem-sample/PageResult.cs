using System.Collections.Generic;

namespace UpdateSalesItem
{
    internal class PageResult<T>
    {
        public IEnumerable<SalesItemResource> Items { get; internal set; }
    }
}