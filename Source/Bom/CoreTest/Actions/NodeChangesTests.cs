using System;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Bom.Core.DataAccess;
using Bom.Core.Model;

namespace Core.Actions
{
    public class NodeChangesTests : IDisposable
    {
        public NodeChangesTests()
        {
            this.Context = TestHelpers.GetModelContext(true);
            var dataFactory = new TestDataFactory();
            RootNode = dataFactory.CreateSampleNodes();
        }
        private Bom.Core.Data.ModelContext Context { get; }

        private TreeNode<MemoryNode> RootNode {get;}


        [Fact]
        public void Simple_move_works()
        {
            EnsureSampleData(Context);

            var memNode = RootNode.First(x => x.Level == 2);
            var p = Context.GetPaths().First(x => x.Node.Title == memNode.Data.Title);
    
            var moved = MoveNodePath(memNode.Data.Title, "", false);

        }

        private Path MoveNodePath(string moveTitle, string newParentTitle, bool moveChildrenToo)
        {
            var moveNode = Context.GetPaths().First(x => x.Node.Title == moveTitle);
            var newParentNode = Context.GetPaths().First(x => x.Node.Title == newParentTitle);
            var prov = new PathNodeProvider(Context);
            var movedPath = prov.MovePath(moveNode.PathId, newParentNode.PathId, moveChildrenToo);
            return movedPath;
        }


        [Fact]
        public void Delete_root_with_children_will_throw()
        {
            try
            {
                var root = EnsureSampleData(Context);
                var prov = new PathNodeProvider(Context);
                prov.DeletePath(root.PathId, true, null);
                Assert.True(false); // make it fail
            }
            catch (Exception ex)
            {
                Console.WriteLine("Expected error happened:" + ex);
            }
        }

        private Bom.Core.Model.Path EnsureSampleData(Bom.Core.Data.ModelContext context)
        {
            var preparer = new TestDataPreparer(context);
            var dbRootNode = preparer.CreateTestData(RootNode);
            return dbRootNode;
        }

        public void Dispose()
        {
            this.Context?.Dispose();
        }
    }
}