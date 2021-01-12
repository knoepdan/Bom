using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Bom.Core.Common;
using Bom.Core.Nodes;
using Bom.Core.Nodes.DbModels;
using Bom.Core.Utils;
using Ch.Knomes.Struct;
using Ch.Knomes.Struct.Testing;

namespace Bom.Core.Data
{
    public class PathQueriesTests : IDisposable
    {
        public PathQueriesTests()
        {
            this.Context = TestHelpers.GetModelContext(true);
            RootNode = TestDataFactory.CreateSampleNodes(MaxLevel, NofChildrenPerNode);
        }

        public const int MaxLevel = 5;

        public const int NofChildrenPerNode = 2;

        private ModelContext Context { get; set; }

        private TreeNode<SimpleNode> RootNode { get; }


        [Fact]
        public void Path_queries_work()
        {
            lock (DbLockers.DbLock)
            {
                EnsureSampleData(Context, RootNode, true);

                // memory nodes
                var memRoot = this.RootNode;
                var memNode = RootNode.Descendants.First(n => n.Level == MaxLevel-2 && n.Siblings.Count > 0); // 1a-2a-3a-4a-5a 
                var memLeaveNode = RootNode.Descendants.First(n => n.Children.Count == 0);
 
                // db nodes
                Path? dbRoot = this.Context.Paths?.First(x => x.Node != null && x.Node.Title == memRoot.Data.Title);
                var dbNode = this.Context.Paths?.First(x => x.Node != null && x.Node.Title == memNode.Data.Title);
                var dbLeaveNode = this.Context.Paths?.First(x => x.Node != null && x.Node.Title == memLeaveNode.Data.Title);
                if(dbRoot == null || dbNode == null || dbLeaveNode == null)
                {
                    throw new InvalidOperationException("Queried nodes/paths are null which should not be possible");
                }
                Assert.True(dbRoot.IsRoot() && !dbNode.IsRoot() && !dbLeaveNode.IsRoot());
                Assert.True(IsTheSame(dbRoot, memRoot) && memRoot.Parent == null);
                Assert.True(IsTheSame(dbNode, memNode));
                Assert.True(IsTheSame(dbLeaveNode, memLeaveNode));

                // Descendants
                var children = this.Context.Paths.Descendants(dbRoot, 2).ToList();
                var memChildren = memRoot.Descendants.Where(x => x.Level <= memRoot.Level + 2).ToList();
                Assert.True(IsTheSame(children, memChildren));
                var children2 = this.Context.Paths.Descendants(dbRoot, 3).ToList();
                var memChildren2 = memRoot.Descendants.Where(x => x.Level <= memRoot.Level + 3).ToList();
                Assert.True(IsTheSame(children, memChildren) && memChildren2.Count > memChildren.Count);

                //GetSiblings
                var siblings = this.Context.Paths.Siblings(dbNode).ToList();
                var memSiblings = memNode.Siblings;
                Assert.True(IsTheSame(siblings, memSiblings));
                Assert.True(IsTheSame(this.Context.Paths.Siblings(dbLeaveNode).ToList(), memLeaveNode.Siblings));

                // DirectParent
                Assert.True(IsTheSame(this.Context.Paths.DirectParent(dbNode), memNode.Parent));
                Assert.True(this.Context.Paths.DirectParent(dbRoot) == null);

                // Ancestors
                var ancestors = this.Context.Paths.Ancestors(dbNode).ToList();
                var memAncestors = memNode.Ancestors;
                Assert.True(IsTheSame(ancestors, memAncestors));
                var ancestors2 = this.Context.Paths.Ancestors(dbLeaveNode, 2).ToList();
                var memAncestors2 = memLeaveNode.Ancestors.Where(x => x.Level >= memLeaveNode.Level - 2).ToList();
                Assert.True(IsTheSame(ancestors2, memAncestors2) && memAncestors2.All(x => x.Parent != null));

                // DbRoot (could be improved by testing with more roots)
                var allDbRoots = this.Context.Paths.AllRootPaths().ToList();
                Assert.True(allDbRoots.Count == 1);
                Assert.True(IsTheSame(allDbRoots.First(), memRoot));
            }
        }
         

        private bool IsTheSame(IEnumerable<Path> dbPathsCol, IEnumerable<TreeNode<SimpleNode>> memNodesCol)
        {
            var dbPaths = dbPathsCol.OrderBy(x => x.Node.Title).ToList();
            var memNodes = memNodesCol.OrderBy(x => x.Data.Title).ToList();
            if(dbPaths.Count != memNodes.Count)
            {
                return false;
            }
            for(int i = 0; i < dbPaths.Count; i++)
            {
                var dbPath = dbPaths[i];
                var memPath = memNodes[i];
                if (!IsTheSame(dbPath, memPath))
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsTheSame(Path? dbPath, TreeNode<SimpleNode>? memNode)
        {
            if(dbPath == null)
            {
                throw new ArgumentNullException(nameof(dbPath));
            }
            if(memNode == null)
            {
                throw new ArgumentNullException(nameof(memNode));
            }

            if(dbPath.Node.Title == memNode.Data.Title && dbPath.Level == memNode.Level)
            {
                return true;
            }
            return false;
        }

        private static Path EnsureSampleData(ModelContext context, TreeNode<SimpleNode> rootNode, bool cleanDatabase)
        {
            var preparer = new TestDataPreparer(context);
            var dbRootNode = preparer.CreateTestData(rootNode, cleanDatabase);
            return dbRootNode;
        }

        public void Dispose()
        {
            this.Context?.Dispose();
        }
    }
}