using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jasmin.IntegrationSample
{
    internal class CompanyResource
    {
        /// <summary>
        /// Gets or sets the serie.
        /// </summary>
        [JsonProperty("companyKey")]
        public string CompanyKey { get; set; }
    }
}
