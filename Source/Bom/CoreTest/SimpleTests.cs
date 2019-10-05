using System;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Ch.Knomes.Struct.Testing;

namespace Bom.Core
{
    public class SimpleTests
    {
        [Fact]
        public void Database_is_accessible()
        {
            var context = TestHelpers.GetModelContext();
            var firstPath = context.GetPaths().FirstOrDefault();
            var nodeOfPath = firstPath?.Node;
            var firstNode = context.GetNodes().FirstOrDefault();
            Console.WriteLine("value1", "value2", $"Path: {firstPath}", $"firstNode: {firstNode}");
        }
      
        [Fact]
        public void Database_clean_does_not_throw()
        {
            try
            {
                lock (DbLockers.DbLock)
                {
                    using (var context = TestHelpers.GetModelContext(true))
                    {
                        var preparer = new TestDataPreparer(context);
                        preparer.CleanTestDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }

        //[Fact]
        //public void Setup_test_data_works()
        //{
        //    try
        //    {
        //        using (var context = TestHelpers.GetModelContext(true))
        //        {
        //            var rootNode = TestDataFactory.CreateSampleNodes(2, 2);
        //            var preparer = new TestDataPreparer(context);
        //            preparer.CreateTestData(rootNode);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //        throw;
        //    }
        //}

    }
}
