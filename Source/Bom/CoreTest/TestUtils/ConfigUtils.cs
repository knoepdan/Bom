using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Bom.Core.TestUtils
{
    public static class ConfigUtils
    {
        static ConfigUtils(){

            }

        public static IConfigurationRoot GetIConfigurationRoot()
        {
            var basePath = TestHelpers.ExecutionLocation;
            var root = GetIConfigurationRoot(basePath);
            return root;
        }

        private static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            // https://weblog.west-wind.com/posts/2018/Feb/18/Accessing-Configuration-in-NET-Core-Test-Projects

            var basePath = TestHelpers.ExecutionLocation;
            var appSettingsFile = System.IO.Path.Combine(basePath, "appsettings.json");
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
              
                .AddJsonFile("appsettings.json", optional: true)
                //.AddUserSecrets("e3dfcccf-0cb3-423a-b302-e3e92e95c128")
             //   .AddEnvironmentVariables()
                .Build();
        }

        public static string ConnectionString
        {
            get
            {
                var config = GetIConfigurationRoot();
                var connection = config.GetConnectionString("DbContext"); // example: "Data Source=localhost;Initial Catalog=aa;persist security info=True;user id=sa;password=abc123%"
                return connection;
            }
        }
    }
}
