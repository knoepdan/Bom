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
    public class MoveNodeTests : IDisposable
    {
        public MoveNodeTests()
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
        public void Moving_path_works()
        {
            EnsureSampleData(Context, RootNode, true);
           
            // Moving up the tree
            MoveLeaveUp();
            MoveNoneLeaveUp();
            MoveNoneLeaveUpAndMoveChildren();
           
            // Moving to another branch
            MoveNoneLeaveToAnotherBranch();
            MoveNoneLeaveToAnotherBranchWithChildren();

            // Moving down
            MoveNodeDown(); // works because children are not moved
            MoveNodeDownWithChildrenThrows(); // THROWS: must fail because it would create a loop

            // 2 roots
            EnsureSampleData(Context, this.AnimalRootNode, false);
            MoveNodeToAnotherTree();
            MoveNodeToAnotherTreeWithChildren();
            MoveRootToAnotherTreeThrows(); // THROWS: moving a single root node is not allowed because all children would become roots themselves
            MoveRootWithChildrenToAnotherTree();

            // current inconsistency and room for improvment
            // - it is possible to move a root node with its children to another tree (and therefore making origin tree "dissapear") but it is not possible to undo such an operation
            // as it is not possible to move a node out of the tree and create a root node. (either allow newParent be set to null to create a new root or create a new stored procedure)
        }

        private void MoveLeaveUp()
        {
            var leaveNode = RootNode.DescendantsAndI.First(n => n.Level == MaxLevel); // 1a-2a-3a-4a-5a 
            var targetParent = RootNode.DescendantsAndI.Where(n => n.Level == 2).Skip(1).First(); // 1a-2b
            var args = new TestMoveNodeArgs(leaveNode, targetParent, false);

            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNoneLeaveUp()
        {
            var node = RootNode.DescendantsAndI.Where(n => n.Level == 3).First(); // 1a-2a-3a
            var targetParent = RootNode; // 1a 
            var args = new TestMoveNodeArgs(node, targetParent, false);
            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNoneLeaveUpAndMoveChildren()
        {
            var baseNode = RootNode.Children.Skip(1).First();// 1a-2b
            var node = baseNode.DescendantsAndI.Where(n => n.Level == 4 && n.Children.Any()).First();
            var targetParent = baseNode.DescendantsAndI.Where(n => n.Level == 2).First();
            var args = new TestMoveNodeArgs(node, targetParent, true);
            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNoneLeaveToAnotherBranch()
        {
            var node = RootNode.DescendantsAndI.Where(n => n.Level == 4 && n.Children.Any()).First();
            var targetParent = RootNode.DescendantsAndI.Where(n => n.Level == 3  && n.Parent != node.Ancestors.First(a => a.Level == 2)).First();
            var args = new TestMoveNodeArgs(node, targetParent, false);
            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNoneLeaveToAnotherBranchWithChildren()
        {
            var node = RootNode.DescendantsAndI.Where(n => n.Level == 4 && n.Children.Any()).First();
            var targetParent = RootNode.DescendantsAndI.Where(n => n.Level == 3 && n.Parent != node.Ancestors.First(a => a.Level == 2)).First();
            var args = new TestMoveNodeArgs(node, targetParent, true);
            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNodeDown()
        {
            var node = RootNode.DescendantsAndI.Where(n => n.Level == 2 && n.Children.Any()).First();
            var targetParent = RootNode.DescendantsAndI.Where(n => n.Level == 4 && n.Ancestors.Any(a => a == node)).First();
            var args = new TestMoveNodeArgs(node, targetParent, false);
            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNodeToAnotherTree()
        {
            var node = RootNode.DescendantsAndI.Where(n => n.Level == 2 && n.Children.Any()).First();
            var targetParent = AnimalRootNode.DescendantsAndI.Where(n => n.Level == 2).Skip(1).First();
            var args = new TestMoveNodeArgs(node, targetParent, false);
            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNodeToAnotherTreeWithChildren()
        {
            var node = RootNode.DescendantsAndI.Where(n => n.Level == 2 && n.Children.Any()).First();
            var targetParent = AnimalRootNode.DescendantsAndI.Where(n => n.Level == 2).Skip(1).First();
            var args = new TestMoveNodeArgs(node, targetParent, true);
            var movedPath = TestMoveNodePath(args);
        }

        private void MoveRootToAnotherTreeThrows()
        {
            var node = RootNode;
            var targetParent = AnimalRootNode.DescendantsAndI.Where(n => n.Level == 2).First();
            var args = new TestMoveNodeArgs(node, targetParent, false);
            try
            {
                var movedPath = TestMoveNodePath(args);
                Assert.True(1 == 2); // trigger assert fail
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Moving a root node to another tree without moving children is not allow as all the children would become root nodes! Error: {ex}");
                return;
            }
        }

        private void MoveRootWithChildrenToAnotherTree()
        {
            var node = RootNode;
            var targetParent = AnimalRootNode.DescendantsAndI.Where(n => n.Level == 2).First();
            var args = new TestMoveNodeArgs(node, targetParent, true);
            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNodeDownWithChildrenThrows()
        {
            // this must fail on the level of the database
            var node = RootNode.DescendantsAndI.Where(n => n.Level == 2 && n.Children.Any()).First();
            var targetParent = RootNode.DescendantsAndI.Where(n => n.Level == 4 && n.Ancestors.Any(a => a == node)).First();
            var args = new TestMoveNodeArgs(node, targetParent, true);
            try
            {
                var moveNode = Context.GetPaths().First(x => x.Node.Title == args.ToMoveNode.Data.Title);
                var newParentNode = Context.GetPaths().First(x => x.Node.Title == args.NewParentNode.Data.Title);
                var prov = new PathNodeProvider(Context);
                var movedPath = prov.MovePathAndReload(moveNode.PathId, newParentNode.PathId, true);
                Assert.True(1 == 2); // trigger assert fail
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Moving a node down its own children fails when calling provider as this is not allowed! Error: {ex}");
                return;
            }
        }

        private Path TestMoveNodePath(TestMoveNodeArgs args)
        {
            // remember some state before
            var rootBeforeMoving = args.ToMoveNode.Root;
            var childrenBeforeMoving = args.ToMoveNode.Children.ToList();

            // do actual move
            var movedPath = MoveNodePath(args.ToMoveNode.Data.Title, args.NewParentNode.Data.Title, args.MoveChildrenToo);

            // ## perform some checks on inMemory node on expected values (afterwards we will check db nodes)
            if (args.NewParentNode.Root != args.ToMoveNode.Root)
            {
                throw new Exception($"InMemory nodes do not have the same parents: {args.NewParentNode.Data.Title},  moved node root: {args.ToMoveNode.Data.Title}"); // check that both are in same tree
            }
            if (args.ToMoveNode.Parent.Data.Title != args.NewParentNode.Data.Title)
            {
                throw new Exception($"InMemory does not have the expected parent: {args.NewParentNode.Data.Title},  moved node parent: {args.ToMoveNode.Parent.Data.Title}");
            }

            // perform some other simple checks (relativly basic)
            if (args.MoveChildrenToo )
            {
                CheckIfAreTheSameAndThrowIfNot(args.ToMoveNode.Children, childrenBeforeMoving);
            }
            else if (!args.MoveChildrenToo && args.ToMoveNode.Children.Count > 0)
            {
                throw new Exception($"InMemory moved node has  {args.ToMoveNode.Children.Count} children but shoulde have none "); // check that both are in same tree
            }

            // compare with DB node (not possible to compare string representation as order of children is not guaranteed to be the same)
            var inMemoryRoot = args.NewParentNode.Root;
            var dbRoot = this.Context.GetPaths().First(p => p.Node.Title == inMemoryRoot.Data.Title);
            var allNodes = this.Context.GetPaths().GetChildren(dbRoot, 9999).ToList(); // level so high we get all
            allNodes.Insert(0, dbRoot);
            var dbRootInMemory = TreeNodeUtils.CreateInMemoryModel(allNodes).First();
            bool areEqual = dbRootInMemory.AreDescendantsAndIEqual(inMemoryRoot, (node, simpleNode) => { return node.Data.Node.Title == simpleNode.Data.Title; });
            if(!areEqual)
            {
                throw new Exception("Nodes do not match"); 
            }

            // checked if moved from one tree to another
            if(rootBeforeMoving.Data.Title != args.ToMoveNode.Root.Data.Title) 
            {
                // we need to check the tree also
                var origTreeRoot = this.Context.GetPaths().First(p => p.Node.Title == rootBeforeMoving.Data.Title);
                var allOrigNodes = this.Context.GetPaths().GetChildren(origTreeRoot, 9999).ToList(); // level so high we get all
                allOrigNodes.Add(origTreeRoot);

                // compare count
                var allPathsCount = this.Context.GetPaths().Count();
                int inMemoryCount = inMemoryRoot.DescendantsAndI.Count() + allOrigNodes.Count;
                if(rootBeforeMoving == args.ToMoveNode)
                {
                    inMemoryCount = inMemoryRoot.DescendantsAndI.Count(); // when we moved the root, allOrigNodes is part of the new tree too and may not be counted
                }
                if (allPathsCount != inMemoryCount)
                {
                    throw new Exception($"Counts do not match. Total number of paths: {allPathsCount}, Orig-Tree: {allOrigNodes.Count}, Target-Tree: {inMemoryRoot.DescendantsAndI.Count()}");
                }

                var dbRootOrigInMemory = TreeNodeUtils.CreateInMemoryModel(allOrigNodes).First();
                bool areOrigNodesEqual = dbRootOrigInMemory.AreDescendantsAndIEqual(rootBeforeMoving, (node, simpleNode) => { return node.Data.Node.Title == simpleNode.Data.Title; });
                if (!areOrigNodesEqual)
                {
                    throw new Exception("Original tree nodes do not match");
                }
            }
            return movedPath;
        }

        private Path MoveNodePath(string moveTitle, string newParentTitle, bool moveChildrenToo)
        {
            var moveNode = Context.GetPaths().First(x => x.Node.Title == moveTitle);
            var newParentNode = Context.GetPaths().First(x => x.Node.Title == newParentTitle);
            var prov = new PathNodeProvider(Context);
            var movedPath = prov.MovePathAndReload(moveNode.PathId, newParentNode.PathId, moveChildrenToo);
            //this.Context.SaveChanges(); not necessary.. is saved because of stored procedure!
            System.Diagnostics.Debug.WriteLine($"Moved node {moveTitle} to new parent {newParentTitle}   (old pathId: {moveNode.PathId} new pathId: {movedPath.PathId})");

            // keep in memory construct in sync
            var inMemoryMoveNode = this.RootNode.DescendantsAndI.First(n => n.Data.Title == moveTitle);
            var inMemoryNewParentNode = this.RootNode.DescendantsAndI.FirstOrDefault(n => n.Data.Title == newParentTitle);
            if(inMemoryNewParentNode == null) // null means maybe we move it to another tree
            {
                inMemoryNewParentNode = this.AnimalRootNode.DescendantsAndI.FirstOrDefault(n => n.Data.Title == newParentTitle);
            }
            inMemoryMoveNode.MoveToNewParent(inMemoryNewParentNode, moveChildrenToo);

            // new context otherwise we might get wrong data (important)
            this.Context.Dispose();
            this.Context = TestHelpers.GetModelContext(true);

            return movedPath;
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