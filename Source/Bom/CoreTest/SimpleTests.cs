using System;
using System.Linq;
using Xunit;
using Core.TestUtils;
using Core.TestUtils.Init;

namespace Core
{
    public class SimpleTests
    {
        [Fact]
        public void CanAccessDatabase()
        {
            var context = TestHelpers.GetModelContext();
            var firstPath = context.GetPaths().FirstOrDefault();
            var nodeOfPath = firstPath.Node;
            var firstNode = context.GetNodes().FirstOrDefault();
            Console.WriteLine("value1", "value2", $"Path: {firstPath}", $"firstNode: {firstNode}");
        }

        [Fact]
        public void InitializeDatabase()
        {
            try
            {
                using (var context = TestHelpers.GetModelContext(true))
                {
                    var initializer = new DataInitializer(context);

                    initializer.AddTestData();
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }


    }
}
