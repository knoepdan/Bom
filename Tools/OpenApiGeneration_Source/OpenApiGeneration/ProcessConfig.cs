using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenApiGeneration.CSharp;

namespace OpenApiGeneration
{
    public class ProcessConfig
    {

        [JsonProperty("clientConfig")]
        public CSharpClientConfig ClientConfig { get; set; } = new CSharpClientConfig();
    }
}
