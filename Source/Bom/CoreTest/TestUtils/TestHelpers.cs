using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Bom.Core.Common;
using Microsoft.EntityFrameworkCore;

namespace Bom.Core.TestUtils
{
    public class TestHelpers
    {
        public static string ExecutionLocation
        {
            get
            {
                // https://stackoverflow.com/questions/23515736/how-to-refer-to-test-files-from-xunit-tests-in-visual-studio
                string? codeBase = Assembly.GetExecutingAssembly().Location;
                if (codeBase == null)
                {
                    throw new Exception("Could not find code base from executing assembly");
                }
                var codeBaseUrl = new Uri(codeBase);
                var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
                var dirPath = Path.GetDirectoryName(codeBasePath);
                return dirPath != null ? dirPath : ""; // path of the actual bin dir of test assembly

                // return Path.Combine(dirPath, relativePath); //  relativePath is the path relative to the Bin\ directory.
            }
        }


        private static ModelContext? _cachedContext;
        public static ModelContext GetModelContext(bool newContext = true)
        {
            if (newContext || _cachedContext == null)
            {
                // dont dispose old cached context.. it may still be in use
                var connectionString = ConfigUtils.ConnectionString;
                var options = new DbContextOptionsBuilder<ModelContext>();
                options.UseSqlServer(connectionString);
                options.UseLazyLoadingProxies();
                _cachedContext = new ModelContext(options.Options);
            }

            return _cachedContext;
        }

    }
}