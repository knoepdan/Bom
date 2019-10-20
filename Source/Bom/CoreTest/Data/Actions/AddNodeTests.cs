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
    public class AddNodeTests : IDisposable
    {
        public AddNodeTests()
        {
            this.Context = TestHelpers.GetModelContext(true);
            RootNode = TestDataFactory.CreateSampleNodes(MaxLevel, NofChildrenPerNode, "");
        }
        private const string SecondPrefix = "ZZZ_";

        public const int MaxLevel = 3;

        public const int NofChildrenPerNode = 2;

        private Bom.Core.Data.ModelContext Context { get; set; }

        private TreeNode<SimpleNode> RootNode { get; }

        [Fact]
        public void Adding_path_works()
        {
            lock (DbLockers.DbLock)
            {
                EnsureSampleData(Context, RootNode, true);

                // Moving up the tree
                AddNewPathForNode();

                // create new root
                AddNewRootPathForNode();

                // create duplicate path (should lead to an exception)
                AddSamePathAgainThrows();

                this.Context.Dispose();
            }
        }

        private void AddNewPathForNode()
        {
            var node = RootNode.DescendantsAndI.First(n => n.Level == MaxLevel - 1); // 1a-2a-3a-4a-5a 
            var parentNode = RootNode.DescendantsAndI.First(x => x.Data.Title != node.Data.Title && x.Level == 2);
            AddNewPathForNodeTest(node, parentNode);
        }

        private void AddNewRootPathForNode()
        {
            this.Context.Dispose();
            this.Context = TestHelpers.GetModelContext(true);

            var node = RootNode.DescendantsAndI.First(n => n.Level == MaxLevel - 1); // 1a-2a-3a-4a-5a 
            var path = AddNewPathForNodeTest(node, null);

            Assert.True(path.IsRoot());
        }

        private void AddSamePathAgainThrows()
        {
            this.Context.Dispose();
            this.Context = TestHelpers.GetModelContext(true);

            int nofPaths = this.Context.Paths.Count();
            try
            {
                var node = RootNode.DescendantsAndI.Skip(1).First(n => n.Level == MaxLevel - 1);
                var parentNode = node.Parent; // same parent
                AddNewPathForNodeTest(node, parentNode);
                Assert.True(1 == 2, "Code was not supposed to reach this point"); // trigger fail
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine("Expected error: " + ex.Message);
                this.Context.Dispose();
                this.Context = TestHelpers.GetModelContext(true);
                Assert.Equal(nofPaths, this.Context.Paths.Count()); // no paths created
            }
        }


        private Path AddNewPathForNodeTest(TreeNode<SimpleNode> node, TreeNode<SimpleNode>? newParentNode)
        {
            if (newParentNode != null && newParentNode.Data.Title == node.Data.Title)
            {
                throw new ArgumentException("newParentNode may not be the same as node", nameof(newParentNode));
            }

            // db data to compare afterwards;
            var allNodesWithSameTitleOrig = Context.Paths.Where(x => x.Node != null && x.Node.Title == node.Data.Title).ToList();
            var pathCountOrig = Context.Paths.Count();
            var nodeCountOrig = Context.Nodes.Count();


            // get db data and act
            var dbPath = Context.Paths.First(x => x.Node != null && x.Node.Title == node.Data.Title);
            Path? dbNewParent = null;
            if (newParentNode != null)
            {
                dbNewParent = Context.Paths.First(x => x.Node != null && x.Node.Title == newParentNode.Data.Title); // some other node
            }

            var prov = new PathNodeProvider(Context);
            var createdPath = prov.AddPathToNode(dbPath.Node, dbNewParent, true);

            // reload with new context (important)
            this.Context.Dispose();
            this.Context = TestHelpers.GetModelContext(true);
            dbPath = Context.Paths.First(x => x.Node != null && x.Node.Title == node.Data.Title);

            // Tests
            Assert.True(dbPath.Node.NodeId == createdPath.Node.NodeId && dbPath.PathId != createdPath.PathId);
            Assert.True((dbNewParent == null) == createdPath.IsRoot());
            var parent = Context.Paths.DirectParent(createdPath);
            Assert.True(dbNewParent == null && parent == null || dbNewParent != null && parent != null);
            if (parent != null && dbNewParent != null)
            {
                Assert.True(dbNewParent.NodePathString == parent.NodePathString);
            }
            var descendants = Context.Paths.Descendants(createdPath, 1).ToList();
            Assert.True(descendants.Count == 0);

            // other check with db data
            var allNodesWithSameTitleNew = Context.Paths.Where(x => x.Node != null && x.Node.Title == node.Data.Title).ToList();
            var pathCountNew = Context.Paths.Count();
            var nodeCountNew = Context.Nodes.Count();
            Assert.Equal(allNodesWithSameTitleOrig.Count + 1, allNodesWithSameTitleNew.Count);
            Assert.Equal(pathCountOrig + 1, pathCountNew);
            Assert.Equal(nodeCountOrig, nodeCountNew);

            return createdPath;
        }



        private void CompareAllInMemoryAndAllDbNodes(IEnumerable<TreeNode<SimpleNode>> rootNodes)
        {
            //  no check all nodes in db an in memory.. all the trees must be equal
            var allNodes = this.Context.Paths.ToList(); // level so high we get all
            var dbRoots = TreeNodeUtils.CreateInMemoryModel(allNodes);
            var memRoots = new List<TreeNode<SimpleNode>>(rootNodes.Distinct());
            if (memRoots.Count != dbRoots.Count)
            {
                throw new Exception($"The number of roots in database {dbRoots.Count} and inMemory {memRoots.Count} are not equal!");
            }
            foreach (var dbRoot in dbRoots)
            {
                var inMemoryRoot = memRoots.FirstOrDefault(x => x.Data.Title == dbRoot.Data.Node?.Title);
                if (inMemoryRoot == null)
                {
                    throw new Exception("Could not find in memory root with title: " + dbRoot.Data.Node?.Title);
                }
                bool areOrigNodesEqual = inMemoryRoot.AreDescendantsAndIEqual(dbRoot, (node, simpleNode) => { return node.Data.Title == simpleNode.Data.Node?.Title; });
                if (!areOrigNodesEqual)
                {
                    throw new Exception($"Trees are not equal (Root: {inMemoryRoot.Data.Title})");
                }
            }
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