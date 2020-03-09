using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    internal class ListResponse<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }
}
