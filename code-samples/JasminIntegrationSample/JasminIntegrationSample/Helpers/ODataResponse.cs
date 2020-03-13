using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    internal class ODataResponse<T>
    {
        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}
