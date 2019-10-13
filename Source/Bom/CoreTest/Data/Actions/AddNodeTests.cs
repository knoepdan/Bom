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
            RootNode = TestDataFactory.CreateSampleNodes(MaxLevel, NofChildrenPerNode);
            AnimalRootNode = TestDataFactory.CreateSampleAnimalNodes();
        }

        public const int MaxLevel = 5;

        public const int NofChildrenPerNode = 2;

        private Bom.Core.Data.ModelContext Context { get; set; }

        private TreeNode<SimpleNode> RootNode { get; }

        private TreeNode<SimpleNode> AnimalRootNode { get; }


        [Fact]
        public void Adding_path_works()
        {
            lock (DbLockers.DbLock)
            {
                EnsureSampleData(Context, RootNode, true);

                // Moving up the tree
                AddNewPathForNode();
            

                // final test
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
            throw new NotImplementedException("TODO");
            //var leaveNode = RootNode.DescendantsAndI.First(n => n.Level == MaxLevel); // 1a-2a-3a-4a-5a 
            //var targetParent = RootNode.DescendantsAndI.Where(n => n.Level == 2).Skip(1).First(); // 1a-2b
            //var args = new TestMoveNodeArgs(leaveNode, targetParent, false);

            //var movedPath = TestMoveNodePath(args);
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