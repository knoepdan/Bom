using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Bom.Core.Data;
using Bom.Core.Model;
using Bom.Core.Data.Actions.Utils;
using Bom.Core.Utils;
using Ch.Knomes.Struct;
using Ch.Knomes.Struct.Testing;

namespace Bom.Core.Data.Actions
{
    public class OrphanTests : IDisposable
    {
        public OrphanTests()
        {
            this.Context = TestHelpers.GetModelContext(true);
            RootNode = TestDataFactory.CreateSampleNodes(MaxLevel, NofChildrenPerNode);
        }

        public const int MaxLevel = 5;

        public const int NofChildrenPerNode = 2;

        private Bom.Core.Data.ModelContext Context { get; set; }

        private TreeNode<SimpleNode> RootNode { get; }

        [Fact]
        public void CheckOrphans()
        {
            lock (DbLockers.DbLock)
            {
                var rootNodes = new List<TreeNode<SimpleNode>>();
                rootNodes.Add(this.RootNode.Root);

                EnsureSampleData(Context, RootNode, true);

                // create orphan and delete node
                CreatingOrphanedNodeAndFinallyDeleteNode();

                // create orphan and add path to orphan (so undo making it an orphan)
                CreatingOrphanedNodeAndReaddPath();
            }
        }

        private void CreatingOrphanedNodeAndFinallyDeleteNode()
        {
            var orphansOrig = this.Context.Nodes.Orphans().ToList();
            Assert.True(orphansOrig.Count == 0);

            var node = RootNode.DescendantsAndI.First(n => n.Level == MaxLevel - 1); // 1a-2a-3a-4a-5a 
            var parentNode = RootNode.DescendantsAndI.First(x => x.Data.Title != node.Data.Title && x.Level == 2);
            var orphanNode = CreateOrphanedNode(node, parentNode);


            var orphans = this.Context.Nodes.Orphans().ToList();
            Assert.True(orphans.Count == 1);
            Assert.True(orphans[0].NodeId == orphanNode.NodeId);


            //  Remove node (ensure no there are no orphans anymore)
            this.Context.Nodes?.Remove(orphans[0]);
            this.Context.SaveChanges();
            this.Context = TestHelpers.GetModelContext(true);
            Assert.True(this.Context.Nodes.Orphans().Count() == 0);
        }

        private void CreatingOrphanedNodeAndReaddPath()
        {
            var orphansOrig = this.Context.Nodes.Orphans().ToList();
            Assert.True(orphansOrig.Count == 0);

            var node = this.Context.Paths.First(x => x.Level > 1);
            var parentNode = this.Context.Paths.First(x => x.Level > 1 && x.PathId != node.PathId);
            var orphanNode = CreateOrphanedNode(node, parentNode);

            var orphans = this.Context.Nodes.Orphans().ToList();
            Assert.True(orphans.Count == 1);
            Assert.True(orphans[0].NodeId == orphanNode.NodeId);


            //  Add path again
            var randomNewParentPath = this.Context.Paths.First(x => x.Level > 1);
            var provider = new PathNodeProvider(this.Context);
            provider.AddPathToNode(orphans[0], randomNewParentPath);
            Assert.True(this.Context.Nodes.Orphans().Count() == 0);
            Assert.True(this.Context.Paths.Descendants(randomNewParentPath, 1).Any(x => x.NodeId == orphanNode.NodeId));
        }

        private Node CreateOrphanedNode(TreeNode<SimpleNode> node, TreeNode<SimpleNode>? newParentNode)
        {
            var dbPath = Context.Paths.First(x => x.Node != null && x.Node.Title == node.Data.Title);
            Path? dbNewParent = null;
            if (newParentNode != null)
            {
                dbNewParent = Context.Paths.First(x => x.Node != null && x.Node.Title == newParentNode.Data.Title); // some other node
            }
            var orphanedNode = CreateOrphanedNode(dbPath, dbNewParent);
            return orphanedNode;
        }


        private Node CreateOrphanedNode(Path dbPath, Path? dbNewParent)
        {
            // prepare
            var dbPathsOriginal = Context.Paths.Where(x => x.Node != null && x.NodeId == dbPath.NodeId).ToList();
            if (dbPathsOriginal.Count > 1)
            {
                throw new Exception($"Testmethod expectes to only have one path for node {dbPath.NodeId}"); // test doesnt support anything other
            }

            var prov = new PathNodeProvider(Context);

            // First add a path
            var createdPath = prov.AddPathToNode(dbPath.Node, dbNewParent, true);

            // reload with new context (important)
            this.Context = TestHelpers.GetModelContext(true);
            var dbPathsAfterAdd = Context.Paths.Where(x => x.Node != null && x.NodeId == dbPath.NodeId).ToList();
            Assert.True(dbPathsAfterAdd.Count == dbPathsOriginal.Count + 1 && dbPathsAfterAdd.Any(y => y.PathId == createdPath.PathId)
                && dbPathsAfterAdd.Any(y => y.PathId == dbPath.PathId)); // only a minimal test as this is better tested elsewhere

            // delete node
            prov.DeletePath(dbPathsAfterAdd.First(x => x.PathId != createdPath.PathId), false, false);
            this.Context = TestHelpers.GetModelContext(true);
            dbPathsAfterAdd = Context.Paths.Where(x => x.Node != null && x.NodeId == dbPath.NodeId).ToList();
            Assert.True(dbPathsAfterAdd.Count == dbPathsOriginal.Count && dbPathsAfterAdd.Any(y => y.PathId == createdPath.PathId)); // only a minimal test as this is better tested elsewhere

            prov.DeletePath(createdPath.PathId, false, false);
            this.Context = TestHelpers.GetModelContext(true);
            dbPathsAfterAdd = Context.Paths.Where(x => x.Node != null && x.NodeId == dbPath.NodeId).ToList();
            Assert.True(dbPathsAfterAdd.Count == dbPathsOriginal.Count - 1 && !dbPathsAfterAdd.Any(y => y.PathId == dbPath.PathId)); // only a minimal test as this is better tested elsewhere
            Assert.True(dbPathsAfterAdd.Count == 0);

            var orphanedNode = Context.Nodes.First(n => n.NodeId == dbPath.NodeId);
            return orphanedNode;
        }
        private static Bom.Core.Model.Path EnsureSampleData(Bom.Core.Data.ModelContext context, TreeNode<SimpleNode> rootNode, bool cleanDatabase)
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