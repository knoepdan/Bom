using System;
using System.Linq;
using System.IO;

namespace Ch.Knomes.TestUtils.Resources
{
    public class ResourceUtility
    {
        public static string ReadManifestData<TSource>(string embeddedFileName) where TSource : class
        {
            var type = typeof(TSource);
            return ReadManifestData(type, embeddedFileName);
        }

        public static string ReadManifestData(Type type, string embeddedFileName)
        {
            var assembly = type.Assembly;
            var allResources = assembly.GetManifestResourceNames();
            var resourceName = allResources.First(s => s.EndsWith(embeddedFileName, StringComparison.CurrentCultureIgnoreCase));

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream for embedded filename: " + embeddedFileName);
                }
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}