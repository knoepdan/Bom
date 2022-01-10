using NJsonSchema;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace OpenApiGeneration.CSharp
{
    public class CSharpClientGenerator
    {
        public CSharpClientConfig Config { get; }

        public CSharpClientGenerator(CSharpClientConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            if (string.IsNullOrEmpty(config.InputDocument))
            {
                throw new ArgumentException("No input document provided", nameof(config.InputDocument));
            }
            if (!File.Exists(config.InputDocument))
            {
                throw new ArgumentException($"Passed input document {config.InputDocument} does not exist", nameof(config.InputDocument));
            }
            this.Config = config;
        }

        public string GenerateCSharpClient()
        {
            var json = File.ReadAllText(this.Config.InputDocument);
            var cSharpCode = GenerateCSharpClient(json);
            return cSharpCode;
        }

        private string GenerateCSharpClient(string openApiSpec)
        {
            if (string.IsNullOrWhiteSpace(openApiSpec))
            {
                throw new ArgumentException("No open api spec was passed!", nameof(openApiSpec));
            }
            var document = OpenApiDocument.FromJsonAsync(openApiSpec, "", SchemaType.OpenApi3).Result;

            var settings = GetSettings();
            var generator = new NSwag.CodeGeneration.CSharp.CSharpClientGenerator(document, settings);
            var cSharpCode = generator.GenerateFile();
            return cSharpCode;
        }

        public string GenerateCSharpClientAndSave()
        {
            string cSharpCode = this.GenerateCSharpClient();
            if (!string.IsNullOrWhiteSpace(this.Config.Output))
            {
                File.WriteAllText(this.Config.Output, cSharpCode);
            }
            return this.Config.Output;
        }

        public CSharpClientGeneratorSettings GetSettings()
        {
            var settings = new CSharpClientGeneratorSettings();

            settings.InjectHttpClient = true;
            settings.DisposeHttpClient = false;
            settings.GenerateClientInterfaces = true;
            settings.CSharpGeneratorSettings.GenerateDefaultValues = false;
            settings.GenerateSyncMethods = true;
            settings.GenerateOptionalParameters = true;
            settings.CSharpGeneratorSettings.HandleReferences = true;
            settings.OperationNameGenerator = new NSwag.CodeGeneration.OperationNameGenerators.MultipleClientsFromOperationIdOperationNameGenerator();
            settings.ExposeJsonSerializerSettings = true;
            settings.UseBaseUrl = false; // base url must be passed via httpClient
            settings.GenerateBaseUrlProperty = false; // base url must be passed via httpClient

            settings.CSharpGeneratorSettings.ArrayBaseType = "System.Collections.Generic.List";
            settings.CSharpGeneratorSettings.ArrayInstanceType = "System.Collections.Generic.List";
            settings.CSharpGeneratorSettings.ArrayType = "System.Collections.Generic.IList";

            // custom
            settings.ClientBaseClass = this.Config.ClientBaseClass;
            settings.CSharpGeneratorSettings.Namespace = this.Config.Namespace;
            settings.GenerateExceptionClasses = true; // because we expect exception class to be overriden

            if (!string.IsNullOrWhiteSpace(this.Config.ExceptionClass))
            {
                settings.ExceptionClass = this.Config.ExceptionClass;
                settings.GenerateExceptionClasses = false;
            }
            else
            {
                settings.GenerateExceptionClasses = true; // have it generated
            }
            settings.CSharpGeneratorSettings.TemplateDirectory = this.Config.TemplateDirectory;

            return settings;
        }
    }
}
