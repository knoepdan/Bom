using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Bom.Core.Data;
using Bom.Core.Model;
using Bom.Core.Actions.Utils;
using Bom.Core.Utils;
using Ch.Knomes.Structure;
using Ch.Knomes.Structure.Testing;

namespace Bom.Core.Actions
{
    public class DeleteNodeTests : IDisposable
    {
        public DeleteNodeTests()
        {
            this.Context = TestHelpers.GetModelContext(true);
            RootNode = TestDataFactory.CreateSampleNodes(MaxLevel, NofChildrenPerNode);
        }

        public const int MaxLevel = 5;

        public const int NofChildrenPerNode = 2;

        private Bom.Core.Data.ModelContext Context { get; set; }

        private TreeNode<SimpleNode> RootNode { get; }

        [Fact]
        public void Deleting_path_works()
        {
            lock (DbLockers.DbLock)
            {
                var rootNodes = new List<TreeNode<SimpleNode>>();
                rootNodes.Add(this.RootNode.Root);

                EnsureSampleData(Context, RootNode, true);

                // tests
                DeletingRootThrows();
                DeleteSinglePathWorks();
                DeleteSubTreeWorks();

                // test entire graph
                this.Context.Dispose();
                this.Context = TestHelpers.GetModelContext(true);
                this.CompareAllInMemoryAndAllDbNodes(rootNodes); // method handles duplicates

                // delete root
                DeleteRootWithNoChildrenWorks();
            }
        }

        private void DeleteSinglePathWorks()
        {
            var node = RootNode.DescendantsAndI.Where(n => n.Level == 3).First(); // 1a-2a-3a
            var args = new TestDeleteNodeArgs(node, true, false);
            TestDeleteNodePath(args);

            node = RootNode.DescendantsAndI.Where(n => n.Level == 3).First(); // 1a-2a-3a
            args = new TestDeleteNodeArgs(node, false, false);
            TestDeleteNodePath(args);
        }

        private void DeleteSubTreeWorks()
        {
            var node = RootNode.DescendantsAndI.Where(n => n.Level == 3).First(); // 1a-2a-3a
            var args = new TestDeleteNodeArgs(node, true, true);
            TestDeleteNodePath(args);

            node = RootNode.DescendantsAndI.Where(n => n.Level == 3).First(); // 1a-2a-3a
            args = new TestDeleteNodeArgs(node, false, true);
            TestDeleteNodePath(args);
        }

        private void DeleteRootWithNoChildrenWorks()
        {
            // step 1 remove all children
            foreach(var c in RootNode.Children.ToList())
            {
                var cArgs = new TestDeleteNodeArgs(c, true, true);
                TestDeleteNodePath(cArgs);
            }

            // delete root
            var args = new TestDeleteNodeArgs(RootNode, false, false);
            DeleteNodePath(args);
            Assert.True(this.Context.GetPaths().Count() == 0);
        }


        private void DeletingRootThrows()
        {
            if (!RootNode.Children.Any())
            {
                throw new Exception("Root not is expected to have children");
            }

            // has 2 throw each time (possible to improve clumsy approach)
            int exCounter = 0;
            try
            {
                var args = new TestDeleteNodeArgs(RootNode, false, false);
                DeleteNodePath(args);
            }
            catch
            {
                exCounter++;
            }

            try
            {
                var args = new TestDeleteNodeArgs(RootNode, false, true);
                DeleteNodePath(args);
            }
            catch
            {
                exCounter++;
            }

            try
            {
                var args = new TestDeleteNodeArgs(RootNode, true, false);
                DeleteNodePath(args);
            }
            catch
            {
                exCounter++;
            }

            try
            {
                var args = new TestDeleteNodeArgs(RootNode, true, true);
                DeleteNodePath(args);
            }
            catch
            {
                exCounter++;
            }
            Assert.True(exCounter == 4);
        }

        private void CompareAllInMemoryAndAllDbNodes(IEnumerable<TreeNode<SimpleNode>> rootNodes)
        {
            //  no check all nodes in db an in memory.. all the trees must be equal
            var allNodes = this.Context.GetPaths().ToList(); // level so high we get all
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

        private void TestDeleteNodePath(TestDeleteNodeArgs args)
        {
            var rootCount = this.RootNode.DescendantsAndI.Count();
            var fromDeleteNode = args.ToDeleteNode;
            Assert.Contains(RootNode.DescendantsAndI, x => x.Data.Title == fromDeleteNode.Data.Title);// toDeleteNode is part of RootNode

            var allDbNodes = this.Context.GetNodes().ToList();
            var allNodesDownFromDelNode = allDbNodes.Where(n => fromDeleteNode.DescendantsAndI.Any(x => x.Data.Title == n.Title)).ToList(); // assumption: nodes for same path all have the same title

            // execute action
            DeleteNodePath(args);

            // basic tests (mainly on in memory)
            if (args.DeleteChildrenToo)
            {
                Assert.True(rootCount - fromDeleteNode.DescendantsAndI.Count() == RootNode.DescendantsAndI.Count());
                Assert.True(RootNode.DescendantsAndI.All(x => !fromDeleteNode.DescendantsAndI.Any(d => d.Data.Title == x.Data.Title))); // no longer there
            }
            else
            {
                Assert.True(rootCount - 1 == RootNode.DescendantsAndI.Count());
                Assert.True(RootNode.DescendantsAndI.All(x => x.Data.Title != fromDeleteNode.Data.Title)); // no longer there
            }

            // ultimate test comparing in memory and db paths
            var rootNodes = new List<TreeNode<SimpleNode>>();
            rootNodes.Add(this.RootNode.Root);
            this.CompareAllInMemoryAndAllDbNodes(rootNodes); // method handles duplicates

            // comparing nodes
            var newDbNodes = this.Context.GetNodes().ToList();
            if (!args.AlsoDeleteNode)
            {
                Assert.True(newDbNodes.Count == allDbNodes.Count); // no changes
            }
            else
            {
                // nodes must have been deleted
                Assert.True(newDbNodes.Count < allDbNodes.Count);
                Assert.True(newDbNodes.All(x => x.Title != fromDeleteNode.Data.Title));
                if (args.DeleteChildrenToo)
                {
                    Assert.True(newDbNodes.Count == allDbNodes.Count - allNodesDownFromDelNode.Count);
                    foreach (var desc in fromDeleteNode.Descendants)
                    {
                        Assert.True(newDbNodes.All(x => x.Title != desc.Data.Title)); // deleted
                    }
                }
                else
                {
                    Assert.True(newDbNodes.Count == allDbNodes.Count - allDbNodes.Where(x => x.Title == args.ToDeleteNode.Data.Title).Count()); // assumption : nodes always have the same title
                    foreach (var desc in fromDeleteNode.Descendants)
                    {
                        Assert.Contains(newDbNodes, x => x.Title == desc.Data.Title); // must still be there
                    }
                }
            }
        }

        private void DeleteNodePath(TestDeleteNodeArgs args)
        {
            var deleteNode = Context.GetPaths().First(x => x.Node != null && x.Node.Title == args.ToDeleteNode.Data.Title);
            var prov = new PathNodeProvider(Context);
            prov.DeletePath(deleteNode, args.AlsoDeleteNode, args.DeleteChildrenToo);
            //this.Context.SaveChanges(); not necessary.. is saved because of stored procedure!
            System.Diagnostics.Debug.WriteLine($"Node {args.ToDeleteNode.Data.Title} was deleted   (also deleteNode: {args.AlsoDeleteNode}, delete children too: {args.DeleteChildrenToo})");

            // keep in memory construct in sync
            var inMemoryDeleteNode = args.ToDeleteNode;
            if (args.DeleteChildrenToo)
            {
                if(inMemoryDeleteNode.Parent == null)
                {
                    throw new Exception($"{nameof(inMemoryDeleteNode)} does not have a parent!");
                }
                inMemoryDeleteNode.Parent.RemoveChild(inMemoryDeleteNode);
            }
            else
            {
                inMemoryDeleteNode.ExtractNodeFromTree();
            }

            // new context otherwise we might get wrong data (important)
            this.Context.Dispose();
            this.Context = TestHelpers.GetModelContext(true);
        }

        private void CheckIfAreTheSameAndThrowIfNot(IEnumerable<TreeNode<SimpleNode>> expected, IEnumerable<TreeNode<SimpleNode>> actual, string additionalMsgInCaseOfError = "")
        {
            var expectedTitles = expected.Select(x => x.Data.Title).ToList();
            var actualTitles = actual.Select(x => x.Data.Title).ToList();
            ComparisonUtils.ThrowIfDuplicates(expectedTitles);
            ComparisonUtils.ThrowIfDuplicates(actualTitles);
            bool isResultAsExpected = ComparisonUtils.HasSameContent(actualTitles, expectedTitles); // possible improvment.. replace with simple foreach and also check level
            if (!isResultAsExpected)
            {
                throw new Exception($"Titles of moved node are not as expected! Expected: {string.Join(", ", expectedTitles)} | actual :  {string.Join(", ", actualTitles)} | {additionalMsgInCaseOfError}");
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