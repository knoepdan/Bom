//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using GenerateOpenApi.Utils;
//using NJsonSchema.Infrastructure;
//using NSwag;
//using Namotion.Reflection;
//using NSwag.Generation;
//using NSwag.Generation.AspNetCore;

//namespace OpenApiGeneration.OpenApi
//{
//    public class OpenApiGenerator
//    {
//        public FileConfig Config { get; }


//        public OpenApiGenerator(FileConfig config)
//        {
//            if (config == null)
//            {
//                throw new ArgumentNullException(nameof(config));
//            }
//            if (string.IsNullOrEmpty(config.AssemblyPath))
//            {
//                throw new ArgumentException("No source dll provided", nameof(config.AssemblyPath));
//            }
//            if (!File.Exists(config.AssemblyPath))
//            {
//                throw new ArgumentException($"Source dll {config.AssemblyPath} does not exist", nameof(config.AssemblyPath));
//            }

//            this.Config = config;
//        }

//        public static AspNetCoreOpenApiDocumentGeneratorSettings GetSettings(FileConfig config)
//        {
//            var settings = new NSwag.Generation.AspNetCore.AspNetCoreOpenApiDocumentGeneratorSettings(); //new WebApiOpenApiDocumentGeneratorSettings();
//            settings.DefaultUrlTemplate = config.DefaultUrlTemplate;
//            settings.Title = config.InfoTitle;

//            hardcoded
//            settings.AllowNullableBodyParameters = true;
//            settings.DefaultResponseReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
//            settings.DefaultReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.Null;
//            settings.DefaultDictionaryValueReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
//            settings.IgnoreObsoleteProperties = true;
//            settings.GenerateKnownTypes = true;
//            settings.SchemaType = NJsonSchema.SchemaType.OpenApi3;
//            settings.DefaultEnumHandling = NJsonSchema.Generation.EnumHandling.String;

//            set some reasonable defaults if not set
//            if (string.IsNullOrEmpty(settings.DefaultUrlTemplate))
//            {
//                settings.DefaultUrlTemplate = "api/{controller}/{id?}";
//            }
//            if (string.IsNullOrEmpty(settings.Title))
//            {
//                settings.Title = "OpenApi";
//            }

//            return settings;
//        }

//        public string GenerateOpenApiFile()
//        {
//            var assemblyPath = this.Config.AssemblyPath;
//            var controllerTypes = GetControllerTypes(assemblyPath);
//            var json = GenerateOpenApiFile(controllerTypes);
//            return json;
//        }


//        public string GenerateOpenApiFile(IEnumerable<Type> controllerTypes)
//        {
//            AspNetCoreOpenApiDocumentGeneratorSettings settings = GetSettings(this.Config);
//            var generator = new WebApiOpenApiDocumentGenerator(settings);

//            var generator = new NSwag.Generation.AspNetCore.AspNetCoreOpenApiDocumentGenerator(settings);

//            var xx = new NSwag.Generation.OpenApiDocumentGenerator()

//             https://andrewlock.net/introduction-to-the-apiexplorer-in-asp-net-core/
//        https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-6.0&tabs=visual-studio
//        https://blog.infernored.com/master-web-apis-with-swagger-apiexplorer-and-nswag/
//            generator.GenerateAsync()

//            var document = generator.GenerateForControllersAsync(controllerTypes).Result;
//            PostProcessDocument(document);
//            string json = document.ToJson(NJsonSchema.SchemaType.OpenApi3, Newtonsoft.Json.Formatting.Indented);
//            return json;
//        }

//        public string GenerateOpenApiFileAndSafe()
//        {
//            string json = this.GenerateOpenApiFile();
//            if (!string.IsNullOrWhiteSpace(this.Config.Output))
//            {
//                File.WriteAllText(this.Config.Output, json);
//            }
//            return this.Config.Output;
//        }

//        private void PostProcessDocument(OpenApiDocument document)
//        {
//            post process security
//           var sec = new OpenApiSecurityRequirement();
//            sec.Add("basicAuth", new List<string>());
//            document.Security.Add(sec);

//            var securityScheme = new OpenApiSecurityScheme();
//            securityScheme.Type = OpenApiSecuritySchemeType.Http;
//            securityScheme.Scheme = "basic";
//            document.SecurityDefinitions.Add("basicAuth", securityScheme);

//            post process from config
//            if (this.Config.ServiceHost == ".")
//            {
//                document.Host = string.Empty;
//            }
//            else if (!string.IsNullOrEmpty(this.Config.ServiceHost))
//            {
//                document.Host = this.Config.ServiceHost;
//            }
//        }

//        private IEnumerable<Type> GetControllerTypes(string assemblyPath)
//        {
//            var assembly = Assembly.LoadFrom(assemblyPath);
//            var controllerTypes = GetControllerClasses(assembly);
//            return controllerTypes;
//        }

//        public static IEnumerable<Type> GetControllerClasses(Assembly assembly)
//        {
//            return assembly.ExportedTypes
//                .Where(t => t.GetTypeInfo().IsAbstract == false)
//                .Where(t => t.Name.EndsWith("Controller") ||
//                            t.InheritsFromTypeName("ApiController", TypeNameStyle.Name) ||
//                            t.InheritsFromTypeName("ControllerBase", TypeNameStyle.Name)) // in ASP.NET Core, a Web API controller inherits from Controller
//                .Where(t => t.GetTypeInfo().ImplementedInterfaces.All(i => i.FullName != "System.Web.Mvc.IController")) // no MVC controllers (legacy ASP.NET)
//                .Where(t =>
//                {
//                    return t.GetTypeInfo().GetCustomAttributes()
//                        .SingleOrDefault(a => a.GetType().Name == "ApiExplorerSettingsAttribute")?
//                        .TryGetPropertyValue("IgnoreApi", false) != true;
//                });
//        }


//        #region old net framework (would need different nuget packages)

//        public static WebApiOpenApiDocumentGeneratorSettings GetSettings(FileConfig config)
//        {
//            var settings = new WebApiOpenApiDocumentGeneratorSettings();
//            settings.DefaultUrlTemplate = config.DefaultUrlTemplate;
//            settings.Title = config.InfoTitle;

//            // hardcoded
//            settings.AllowNullableBodyParameters = true;
//            settings.DefaultResponseReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
//            settings.DefaultReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.Null;
//            settings.DefaultDictionaryValueReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
//            settings.IgnoreObsoleteProperties = true;
//            settings.GenerateKnownTypes = true;
//            settings.SchemaType = NJsonSchema.SchemaType.OpenApi3;
//            settings.DefaultEnumHandling = NJsonSchema.Generation.EnumHandling.String;

//            // set some reasonable defaults if not set
//            if (string.IsNullOrEmpty(settings.DefaultUrlTemplate))
//            {
//                settings.DefaultUrlTemplate = "api/{controller}/{id?}";
//            }
//            if (string.IsNullOrEmpty(settings.Title))
//            {
//                settings.Title = "OpenApi";
//            }

//            return settings;
//        }

//        public string GenerateOpenApiFile(IEnumerable<Type> controllerTypes)
//        {
//            WebApiOpenApiDocumentGeneratorSettings settings = GetSettings(this.Config);
//            var generator = new WebApiOpenApiDocumentGenerator(settings);

//            var document = generator.GenerateForControllersAsync(controllerTypes).Result;
//            PostProcessDocument(document);
//            string json = document.ToJson(NJsonSchema.SchemaType.OpenApi3, Newtonsoft.Json.Formatting.Indented);
//            return json;
//        }

//        private IEnumerable<Type> GetControllerTypes(string assemblyPath)
//        {
//            var assembly = Assembly.LoadFrom(assemblyPath);
//            var controllerTypes = GetControllerClasses(assembly);
//            return controllerTypes;
//        }

//        public static IEnumerable<Type> GetControllerClasses(Assembly assembly)
//        {
//            return assembly.ExportedTypes
//                .Where(t => t.GetTypeInfo().IsAbstract == false)
//                .Where(t => t.Name.EndsWith("Controller") ||
//                            t.InheritsFromTypeName("ApiController", TypeNameStyle.Name) ||
//                            t.InheritsFromTypeName("ControllerBase", TypeNameStyle.Name)) // in ASP.NET Core, a Web API controller inherits from Controller
//                .Where(t => t.GetTypeInfo().ImplementedInterfaces.All(i => i.FullName != "System.Web.Mvc.IController")) // no MVC controllers (legacy ASP.NET)
//                .Where(t =>
//                {
//                    return t.GetTypeInfo().GetCustomAttributes()
//                        .SingleOrDefault(a => a.GetType().Name == "ApiExplorerSettingsAttribute")?
//                        .TryGetPropertyValue("IgnoreApi", false) != true;
//                });
//        }

//        #endregion

//    }
//}
