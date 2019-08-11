using System;
using System.Linq;
using Xunit;
using Core.TestUtils;
using Core.Testing;

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
        public void CleanDatabase()
        {
            try
            {
                using (var context = TestHelpers.GetModelContext(true))
                {
                    var preparer = new TestDataPreparer(context);
                    preparer.CleanTestDatabase();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }

        [Fact]
        public void SetupTestdata()
        {
            try
            {
                using (var context = TestHelpers.GetModelContext(true))
                {
                    var dataFactory = new TestDataFactory();
                    var rootNode = dataFactory.CreateSampleNodes();
                    var preparer = new TestDataPreparer(context);
                    preparer.CreateTestData(rootNode);
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
