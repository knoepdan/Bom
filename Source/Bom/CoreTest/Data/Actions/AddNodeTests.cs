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
            SecondRootNode = TestDataFactory.CreateSampleNodes(MaxLevel, NofChildrenPerNode, SecondPrefix);
        }
        private const string SecondPrefix = "ZZZ_";

        public const int MaxLevel = 3;

        public const int NofChildrenPerNode = 2;

        private Bom.Core.Data.ModelContext Context { get; set; }

        private TreeNode<SimpleNode> RootNode { get; }

        private TreeNode<SimpleNode> SecondRootNode { get; }


        [Fact]
        public void Adding_path_works()
        {
            lock (DbLockers.DbLock)
            {
                EnsureSampleData(Context, RootNode, true);
                EnsureSampleData(Context, SecondRootNode, false);

                // Moving up the tree
                AddNewPathForNode();

                // create new root
                AddNewRootPathForNode();

                // create duplicate path (should lead to an exception)
                AddSamePathAgainThrows();

                // final test
                 this.Context.Dispose();
                //this.Context.Dispose();
                //this.Context = TestHelpers.GetModelContext(true);
                //var rootNodes = new List<TreeNode<SimpleNode>>();
                //rootNodes.Add(this.AnimalRootNode.Root);
                //rootNodes.Add(this.RootNode.Root);
                //rootNodes.Add(createdRoot);
                //rootNodes.Add(anotherRootNode);
                //this.CompareAllInMemoryAndAllDbNodes(rootNodes); // method handles duplicates
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

            int nofPaths = this.Context.GetPaths().Count();
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
                Assert.Equal(nofPaths, this.Context.GetPaths().Count()); // no paths created
            }
        }


        private Path AddNewPathForNodeTest(TreeNode<SimpleNode> node, TreeNode<SimpleNode>? newParentNode)
        {
            if (newParentNode != null && newParentNode.Data.Title == node.Data.Title)
            {
                throw new ArgumentException("newParentNode may not be the same as node", nameof(newParentNode));
            }

            // db data to compare afterwards;
            var allNodesWithSameTitleOrig = Context.GetPaths().Where(x => x.Node != null && x.Node.Title == node.Data.Title).ToList();
            var pathCountOrig = Context.GetPaths().Count();
            var nodeCountOrig = Context.GetNodes().Count();


            // get db data and act
            var dbPath = Context.GetPaths().First(x => x.Node != null && x.Node.Title == node.Data.Title);
            Path? dbNewParent = null;
            if (newParentNode != null)
            {
                dbNewParent = Context.GetPaths().First(x => x.Node != null && x.Node.Title == newParentNode.Data.Title); // some other node
            }

            var prov = new PathNodeProvider(Context);
            var createdPath = prov.AddPathToNode(dbPath.Node, dbNewParent, true);

            // reload with new context (important)
            this.Context.Dispose();
            this.Context = TestHelpers.GetModelContext(true);
            dbPath = Context.GetPaths().First(x => x.Node != null && x.Node.Title == node.Data.Title);

            // Tests
            Assert.True(dbPath.Node.NodeId == createdPath.Node.NodeId && dbPath.PathId != createdPath.PathId);
            Assert.True((dbNewParent == null) == createdPath.IsRoot());
            var parent = Context.GetPaths().DirectParent(createdPath);
            Assert.True(dbNewParent == null && parent == null || dbNewParent != null && parent != null);
            if (parent != null && dbNewParent != null)
            {
                Assert.True(dbNewParent.NodePathString == parent.NodePathString);
            }
            var descendants = Context.GetPaths().Descendants(createdPath, 1).ToList();
            Assert.True(descendants.Count == 0);

            // other check with db data
            var allNodesWithSameTitleNew = Context.GetPaths().Where(x => x.Node != null && x.Node.Title == node.Data.Title).ToList();
            var pathCountNew = Context.GetPaths().Count();
            var nodeCountNew = Context.GetNodes().Count();
            Assert.Equal(allNodesWithSameTitleOrig.Count + 1, allNodesWithSameTitleNew.Count);
            Assert.Equal(pathCountOrig + 1, pathCountNew);
            Assert.Equal(nodeCountOrig, nodeCountNew);

            return createdPath;
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


        //private Path TestMoveNodePath(TestMoveNodeArgs args)
        //{
        //    if (args.ToMoveNode == null)
        //    {
        //        throw new ArgumentException($"{nameof(args.ToMoveNode)} may not be null", nameof(args));
        //    }
        //    if (args.NewParentNode == null)
        //    {
        //        throw new ArgumentException($"{nameof(args.NewParentNode)} may not be null", nameof(args));
        //    }
        //    var toMoveNode = args.ToMoveNode;
        //    var newParentNode = args.NewParentNode;

        //    // remember some state before
        //    var rootBeforeMoving = args.ToMoveNode.Root;
        //    var childrenBeforeMoving = args.ToMoveNode.Children.ToList();

        //    // do actual move
        //    var movedPath = MoveNodePath(toMoveNode.Data.Title, newParentNode.Data?.Title, args.MoveChildrenToo);

        //    // ## perform some checks on inMemory node on expected values (afterwards we will check db nodes)
        //    if (args.NewParentNode.Root != args.ToMoveNode.Root)
        //    {
        //        throw new Exception($"InMemory nodes do not have the same parents: {args.NewParentNode.Data.Title},  moved node root: {toMoveNode.Data.Title}"); // check that both are in same tree
        //    }
        //    if (args.ToMoveNode?.Parent?.Data.Title != args.NewParentNode?.Data?.Title)
        //    {
        //        throw new Exception($"InMemory does not have the expected parent: {newParentNode?.Data?.Title},  moved node parent: {toMoveNode?.Parent?.Data.Title}");
        //    }

        //    // perform some other simple checks (relativly basic)
        //    if (args.MoveChildrenToo )
        //    {
        //        CheckIfAreTheSameAndThrowIfNot(toMoveNode.Children, childrenBeforeMoving);
        //    }
        //    else if (!args.MoveChildrenToo && toMoveNode.Children.Count > 0)
        //    {
        //        throw new Exception($"InMemory moved node has  {toMoveNode.Children.Count} children but shoulde have none "); // check that both are in same tree
        //    }

        //    // compare with DB node (not possible to compare string representation as order of children is not guaranteed to be the same)
        //    var inMemoryRoot = newParentNode.Root;
        //    var dbRoot = this.Context.GetPaths().First(p => p.Node != null && p.Node.Title == inMemoryRoot.Data.Title);
        //    var allNodes = this.Context.GetPaths().Descendants(dbRoot, 9999).ToList(); // level so high we get all
        //    allNodes.Insert(0, dbRoot);
        //    var dbRootInMemory = TreeNodeUtils.CreateInMemoryModel(allNodes).First();
        //    bool areEqual = dbRootInMemory.AreDescendantsAndIEqual(inMemoryRoot, (node, simpleNode) => { return node.Data.Node?.Title == simpleNode.Data.Title; });
        //    if(!areEqual)
        //    {
        //        throw new Exception("Nodes do not match"); 
        //    }

        //    // checked if moved from one tree to another
        //    if(rootBeforeMoving.Data.Title != toMoveNode.Root.Data.Title) 
        //    {
        //        // we need to check the tree also
        //        var origTreeRoot = this.Context.GetPaths().First(p => p.Node != null && p.Node.Title == rootBeforeMoving.Data.Title);
        //        var allOrigNodes = this.Context.GetPaths().Descendants(origTreeRoot, 9999).ToList(); // level so high we get all
        //        allOrigNodes.Add(origTreeRoot);

        //        // compare count
        //        var allPathsCount = this.Context.GetPaths().Count();
        //        int inMemoryCount = inMemoryRoot.DescendantsAndI.Count() + allOrigNodes.Count;
        //        if(rootBeforeMoving == args.ToMoveNode)
        //        {
        //            inMemoryCount = inMemoryRoot.DescendantsAndI.Count(); // when we moved the root, allOrigNodes is part of the new tree too and may not be counted
        //        }
        //        if (allPathsCount != inMemoryCount)
        //        {
        //            throw new Exception($"Counts do not match. Total number of paths: {allPathsCount}, Orig-Tree: {allOrigNodes.Count}, Target-Tree: {inMemoryRoot.DescendantsAndI.Count()}");
        //        }

        //        var dbRootOrigInMemory = TreeNodeUtils.CreateInMemoryModel(allOrigNodes).First();
        //        bool areOrigNodesEqual = dbRootOrigInMemory.AreDescendantsAndIEqual(rootBeforeMoving, (node, simpleNode) => { return node.Data.Node?.Title == simpleNode.Data.Title; });
        //        if (!areOrigNodesEqual)
        //        {
        //            throw new Exception("Original tree nodes do not match");
        //        }
        //    }
        //    return movedPath;
        //}

        //private Path MoveNodePath(string moveTitle, string? newParentTitle, bool moveChildrenToo)
        //{
        //    var moveNode = Context.GetPaths().First(x => x.Node != null && x.Node.Title == moveTitle);
        //    var newParentNode = Context.GetPaths().FirstOrDefault(x => x.Node != null && x.Node.Title == newParentTitle);
        //    var prov = new PathNodeProvider(Context);
        //    var movedPath = prov.MovePathAndReload(moveNode.PathId, newParentNode == null ? 0 : newParentNode.PathId, moveChildrenToo);
        //    //this.Context.SaveChanges(); not necessary.. is saved because of stored procedure!
        //    System.Diagnostics.Debug.WriteLine($"Moved node {moveTitle} to new parent {newParentTitle}   (old pathId: {moveNode.PathId} new pathId: {movedPath.PathId})");

        //    // keep in memory construct in sync
        //    var inMemoryMoveNode = this.RootNode.DescendantsAndI.First(n => n.Data.Title == moveTitle);
        //    var inMemoryNewParentNode = this.RootNode.DescendantsAndI.FirstOrDefault(n => n.Data.Title == newParentTitle);
        //    if(inMemoryNewParentNode == null) // null means maybe we move it to another tree
        //    {
        //        inMemoryNewParentNode = this.AnimalRootNode.DescendantsAndI.FirstOrDefault(n => n.Data.Title == newParentTitle);
        //    }
        //    if (inMemoryNewParentNode != null)
        //    {
        //        inMemoryMoveNode.MoveToNewParent(inMemoryNewParentNode, moveChildrenToo);
        //    }else if(newParentTitle == null)
        //    {
        //        if (!moveChildrenToo)
        //        {
        //            inMemoryMoveNode.ExtractNodeFromTree();
        //        }
        //        else
        //        {
        //            if(inMemoryMoveNode.Parent == null)
        //            {
        //                throw new Exception($"{nameof(inMemoryMoveNode)} parent is null!" );
        //            }
        //            inMemoryMoveNode.Parent.RemoveChild(inMemoryMoveNode);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception($"Could not find new parent path in in memory node for node title {newParentTitle}"); // error in test setup
        //    }

        //    // new context otherwise we might get wrong data (important)
        //    this.Context.Dispose();
        //    this.Context = TestHelpers.GetModelContext(true);

        //    return movedPath;
        //}

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