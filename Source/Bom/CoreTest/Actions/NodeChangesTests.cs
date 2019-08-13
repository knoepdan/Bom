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
            using (var context = TestHelpers.GetModelContext(true))
            {
                EnsureSampleData(context);
                var prov = new PathNodeProvider(context);
                // prov.MovePath()
            }
        }

        [Fact]
        public void Delete_root_with_children_will_throw()
        {
            using (var context = TestHelpers.GetModelContext(true))
            {
                var root = EnsureSampleData(context);
                var prov = new PathNodeProvider(context);
                prov.DeletePath(root.PathId, true, null);

                // todo catch excpect failure in unit test 
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
