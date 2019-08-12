using System;
using System.Linq;
using Xunit;
using Core.TestUtils;
using Core.Testing;
using Core.DataAccess;
using Core.Model;

namespace Core.Actions
{
    public class NodeChangesTests
    {
        [Fact]
        public void Simple_move_works()
        {
            try
            {
                using (var context = TestHelpers.GetModelContext(true))
                {
                    EnsureSampleData(context);
                    var prov = new PathNodeProvider(context);
                   // prov.MovePath()
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }

        [Fact]
        public void Delete_works()
        {
            try
            {
                using (var context = TestHelpers.GetModelContext(true))
                {
                    var root = EnsureSampleData(context);
                    var prov = new PathNodeProvider(context);
                    prov.DeletePath(root.PathId, true, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }


        private Model.Path EnsureSampleData(Data.ModelContext context)
        {
            var dataFactory = new TestDataFactory();
            var rootNode = dataFactory.CreateSampleNodes();
            var preparer = new TestDataPreparer(context);
            var dbRootNode = preparer.CreateTestData(rootNode);
            return dbRootNode;
        }

    }
}
