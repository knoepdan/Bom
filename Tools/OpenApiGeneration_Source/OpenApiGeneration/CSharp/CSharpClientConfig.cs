using Newtonsoft.Json;

namespace OpenApiGeneration.CSharp
{
    public class CSharpClientConfig
    {
        [JsonProperty("inputDocument")]
        public string InputDocument { get; set; }

        [JsonProperty("output")]
        public string Output { get; set; }

        [JsonProperty("clientBaseClass")]
        public string ClientBaseClass { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("exceptionClass")]
        public string ExceptionClass { get; set; }

        [JsonProperty("templateDirectory")]
        public string TemplateDirectory { get; set; }
    }
}
