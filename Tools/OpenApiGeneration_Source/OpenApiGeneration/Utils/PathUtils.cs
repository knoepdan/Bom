using System.Reflection;

namespace OpenApiGeneration.Utils
{
    public static class PathUtils
    {
        public static string? GetCurrentDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var currentDir = Path.GetDirectoryName(assembly.Location);
            return currentDir;
        }
    }
}
