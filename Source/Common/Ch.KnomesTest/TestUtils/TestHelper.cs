using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace Ch.Knomes.TestUtils
{
    public class TestHelper
    {
        public static string ExecutionLocation
        {
            get
            {
                // https://stackoverflow.com/questions/23515736/how-to-refer-to-test-files-from-xunit-tests-in-visual-studio
                var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
                var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
                var dirPath = Path.GetDirectoryName(codeBasePath);
                return dirPath; // path of the actual bin dir of test assembly

                // return Path.Combine(dirPath, relativePath); //  relativePath is the path relative to the Bin\ directory.
            }
        }
    }
}
