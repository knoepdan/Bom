using OpenApiGeneration.Utils;
using OpenApiGeneration.CSharp;

namespace OpenApiGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            string configPath = "generateOpenApi.config";
            try
            {
                // handle args
                if (args != null && args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
                {
                    if (args[0] == "saveSampleConfig")
                    {
                        JsonConfigHandler.SaveToJsonFile(new ProcessConfig(), configPath);
                        Console.WriteLine($"Empty sample config safed to: '{configPath}' (current directory)");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    else
                    {
                        configPath = args[0];
                    }
                }

                Process(configPath);

                if (args != null && args.Any(a => a.Equals("waitTillFinish", StringComparison.InvariantCultureIgnoreCase)))
                {
                    Console.WriteLine("Done!");
                    Console.ReadKey();
                }
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                Environment.Exit(-1);
            }
        }

        private static void Process(string configPath)
        {
            if (!File.Exists(configPath))
            {
                Console.WriteLine($"Could not find a configuration file. Path: '{configPath}'");
                Environment.Exit(-1);
            }

            var config = GetConfig(configPath);

            // 1. generate open api json
            //if (config.GenerateOpenApiConfig != null && !string.IsNullOrEmpty(config.GenerateOpenApiConfig.AssemblyPath))
            //{
            //    var openApiGenerator = new OpenApiGenerator(config.GenerateOpenApiConfig);
            //    openApiGenerator.GenerateOpenApiFileAndSafe();
            //    Console.WriteLine($"## Process. Output path of generated open api json file: {config.GenerateOpenApiConfig.Output}");
            //}
            //else
            //{
            //    Console.WriteLine($"## No open api json file generated as not defined in config file");
            //}

            // 2. generate client from open api json file
            if (config.ClientConfig != null && !string.IsNullOrEmpty(config.ClientConfig.InputDocument))
            {
                var clientGenerator = new CSharpClientGenerator(config.ClientConfig);
                clientGenerator.GenerateCSharpClientAndSave();
                Console.WriteLine($"## C# client generated: '{config.ClientConfig.Output}'");
            }
            else
            {
                Console.WriteLine($"## No open api json file generated as not defined in config file");
            }
        }

        private static ProcessConfig GetConfig(string path)
        {
            var config = JsonConfigHandler.GetObjectFromJsonFile<ProcessConfig>(path);
            if (config == null)
            {
                throw new Exception($"Could not generate configuration instance from path: {path}");
            }
            return config;
        }
    }
}


// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
