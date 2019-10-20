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
            lock (DbLockers.DbLock)
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
                MoveRootWithChildrenToAnotherTree(); // will result in one single tree

                // 2. create new roots (is allowed because we also allow to merge trees, without this, it would not be possible to undo this action)
                var createdRoot = MakeSubNodeANewRoot();
                var anotherRootNode = MakeSubNodeWithChildrenANewRoot(createdRoot);
                MakeNodeRootNodeThatIsAlreadyRootThrows(createdRoot, anotherRootNode);

                // final test
                this.Context.Dispose();
                this.Context = TestHelpers.GetModelContext(true);
                var rootNodes = new List<TreeNode<SimpleNode>>();
                rootNodes.Add(this.AnimalRootNode.Root);
                rootNodes.Add(this.RootNode.Root);
                rootNodes.Add(createdRoot);
                rootNodes.Add(anotherRootNode);
                this.CompareAllInMemoryAndAllDbNodes(rootNodes); // method handles duplicates
            }
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
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Moving a root node to another tree without moving children is not allow as all the children would become root nodes! Error: {ex}");
                return;
            }

            // was not in catch if we reach this code
            Assert.True(1 == 2); // trigger assert fail
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
            try
            {
                var moveNode = Context.Paths.First(x => x.Node != null && x.Node.Title == node.Data.Title);
                var newParentNode = Context.Paths.First(x => x.Node != null && x.Node.Title == targetParent.Data.Title);
                var prov = new PathNodeProvider(Context);
                var movedPath = prov.MovePathAndReload(moveNode.PathId, newParentNode.PathId, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Moving a node down its own children fails when calling provider as this is not allowed! Error: {ex}");
                return;
            }

            // was not in catch if we reach this code
            Assert.True(1 == 2); // trigger assert fail
        }

        private TreeNode<SimpleNode> MakeSubNodeANewRoot()
        {
            var node = RootNode.DescendantsAndI.Where(n => n.Level == RootNode.Level + 1 && n.Children.Any()).First();
            var args = new TestMoveNodeArgs(node, null, false); // target is null 
            TestMakeNewRoot(args);
            return args.ToMoveNode;
        }

        private TreeNode<SimpleNode> MakeSubNodeWithChildrenANewRoot(params TreeNode<SimpleNode>[] additionalRootNodes)
        {
            var node = RootNode.DescendantsAndI.Where(n => n.Level == RootNode.Level + 1 && n.Children.Any()).First();
            var args = new TestMoveNodeArgs(node, null, true); // target is null 
            TestMakeNewRoot(args, additionalRootNodes);
            return args.ToMoveNode;
        }

        private void MakeNodeRootNodeThatIsAlreadyRootThrows(params TreeNode<SimpleNode>[] additionalRootNodes)
        {
            var node = RootNode.Root;
            try
            {
                var args = new TestMoveNodeArgs(node, null, false); // target is null 
                TestMakeNewRoot(args, additionalRootNodes);
                Assert.True(1 == 2); // trigger assert fail
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Making a root not of a node that is already root triggers error! Error: {ex}");
            }

            try
            {
                var args = new TestMoveNodeArgs(node, null, true); // target is null 
                TestMakeNewRoot(args, additionalRootNodes);
                Assert.True(1 == 2); // trigger assert fail
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Making a root not of a node that is already root triggers error! (with children) Error: {ex}");
            }
        }

        private void TestMakeNewRoot(TestMoveNodeArgs args, params TreeNode<SimpleNode>[] additionalRootNodes)
        {
            if(args.NewParentNode != null)
            {
                throw new ArgumentException("The new parent path must be null", nameof(args));
            }

            // make new root
            var childrenBeforeMoving = args.ToMoveNode.Children.ToList();
            var movedPathAsRoot = MoveNodePath(args.ToMoveNode.Data.Title, null, args.MoveChildrenToo);

            // quick checks
            if (args.MoveChildrenToo)
            {
                CheckIfAreTheSameAndThrowIfNot(args.ToMoveNode.Children, childrenBeforeMoving);
            }
            else if (!args.MoveChildrenToo && args.ToMoveNode.Children.Count > 0)
            {
                throw new Exception($"InMemory moved node has  {args.ToMoveNode.Children.Count} children but shoulde have none "); // check that both are in same tree
            }
            if(args.ToMoveNode.Parent != null)
            {
                throw new Exception("Inmemory nood is expected to be root and may not have a parent");
            }
            if (args.ToMoveNode.Parent != null)
            {
                throw new Exception("Inmemory nood is expected to be root and may not have a parent");
            }
            if (!movedPathAsRoot.IsRoot())
            {
                throw new Exception("Db path is expected to be root but is not. Path: " + movedPathAsRoot.NodePathString);
            }


            // compare all roots
            var rootNodes = new List<TreeNode<SimpleNode>>();
            rootNodes.Add(this.AnimalRootNode.Root);
            rootNodes.Add(this.RootNode.Root);
            rootNodes.Add(args.ToMoveNode); // new root node
            if (additionalRootNodes != null)
            {
                rootNodes.AddRange(additionalRootNodes);
            }
            this.CompareAllInMemoryAndAllDbNodes(rootNodes); // method handles duplicates
            if (!rootNodes.Any(x => x == args.ToMoveNode))
            {
                throw new Exception("Moved does not seem to be a in memory root node"); // we kind of checked this before when we checked if parent was null
            }
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


        private Path TestMoveNodePath(TestMoveNodeArgs args)
        {
            if (args.ToMoveNode == null)
            {
                throw new ArgumentException($"{nameof(args.ToMoveNode)} may not be null", nameof(args));
            }
            if (args.NewParentNode == null)
            {
                throw new ArgumentException($"{nameof(args.NewParentNode)} may not be null", nameof(args));
            }
            var toMoveNode = args.ToMoveNode;
            var newParentNode = args.NewParentNode;

            // remember some state before
            var rootBeforeMoving = args.ToMoveNode.Root;
            var childrenBeforeMoving = args.ToMoveNode.Children.ToList();

            // do actual move
            var movedPath = MoveNodePath(toMoveNode.Data.Title, newParentNode.Data?.Title, args.MoveChildrenToo);

            // ## perform some checks on inMemory node on expected values (afterwards we will check db nodes)
            if (args.NewParentNode.Root != args.ToMoveNode.Root)
            {
                throw new Exception($"InMemory nodes do not have the same parents: {args.NewParentNode.Data.Title},  moved node root: {toMoveNode.Data.Title}"); // check that both are in same tree
            }
            if (args.ToMoveNode?.Parent?.Data.Title != args.NewParentNode?.Data?.Title)
            {
                throw new Exception($"InMemory does not have the expected parent: {newParentNode?.Data?.Title},  moved node parent: {toMoveNode?.Parent?.Data.Title}");
            }

            // perform some other simple checks (relativly basic)
            if (args.MoveChildrenToo )
            {
                CheckIfAreTheSameAndThrowIfNot(toMoveNode.Children, childrenBeforeMoving);
            }
            else if (!args.MoveChildrenToo && toMoveNode.Children.Count > 0)
            {
                throw new Exception($"InMemory moved node has  {toMoveNode.Children.Count} children but shoulde have none "); // check that both are in same tree
            }

            // compare with DB node (not possible to compare string representation as order of children is not guaranteed to be the same)
            var inMemoryRoot = newParentNode.Root;
            var dbRoot = this.Context.Paths.First(p => p.Node != null && p.Node.Title == inMemoryRoot.Data.Title);
            var allNodes = this.Context.Paths.Descendants(dbRoot, 9999).ToList(); // level so high we get all
            allNodes.Insert(0, dbRoot);
            var dbRootInMemory = TreeNodeUtils.CreateInMemoryModel(allNodes).First();
            bool areEqual = dbRootInMemory.AreDescendantsAndIEqual(inMemoryRoot, (node, simpleNode) => { return node.Data.Node?.Title == simpleNode.Data.Title; });
            if(!areEqual)
            {
                throw new Exception("Nodes do not match"); 
            }

            // checked if moved from one tree to another
            if(rootBeforeMoving.Data.Title != toMoveNode.Root.Data.Title) 
            {
                // we need to check the tree also
                var origTreeRoot = this.Context.Paths.First(p => p.Node != null && p.Node.Title == rootBeforeMoving.Data.Title);
                var allOrigNodes = this.Context.Paths.Descendants(origTreeRoot, 9999).ToList(); // level so high we get all
                allOrigNodes.Add(origTreeRoot);

                // compare count
                var allPathsCount = this.Context.Paths.Count();
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
                bool areOrigNodesEqual = dbRootOrigInMemory.AreDescendantsAndIEqual(rootBeforeMoving, (node, simpleNode) => { return node.Data.Node?.Title == simpleNode.Data.Title; });
                if (!areOrigNodesEqual)
                {
                    throw new Exception("Original tree nodes do not match");
                }
            }
            return movedPath;
        }

        private Path MoveNodePath(string moveTitle, string? newParentTitle, bool moveChildrenToo)
        {
            var moveNode = Context.Paths.First(x => x.Node != null && x.Node.Title == moveTitle);
            var newParentNode = Context.Paths.FirstOrDefault(x => x.Node != null && x.Node.Title == newParentTitle);
            var prov = new PathNodeProvider(Context);
            var movedPath = prov.MovePathAndReload(moveNode.PathId, newParentNode == null ? 0 : newParentNode.PathId, moveChildrenToo);
            //this.Context.SaveChanges(); not necessary.. is saved because of stored procedure!
            System.Diagnostics.Debug.WriteLine($"Moved node {moveTitle} to new parent {newParentTitle}   (old pathId: {moveNode.PathId} new pathId: {movedPath.PathId})");

            // keep in memory construct in sync
            var inMemoryMoveNode = this.RootNode.DescendantsAndI.First(n => n.Data.Title == moveTitle);
            var inMemoryNewParentNode = this.RootNode.DescendantsAndI.FirstOrDefault(n => n.Data.Title == newParentTitle);
            if(inMemoryNewParentNode == null) // null means maybe we move it to another tree
            {
                inMemoryNewParentNode = this.AnimalRootNode.DescendantsAndI.FirstOrDefault(n => n.Data.Title == newParentTitle);
            }
            if (inMemoryNewParentNode != null)
            {
                inMemoryMoveNode.MoveToNewParent(inMemoryNewParentNode, moveChildrenToo);
            }else if(newParentTitle == null)
            {
                if (!moveChildrenToo)
                {
                    inMemoryMoveNode.ExtractNodeFromTree();
                }
                else
                {
                    if(inMemoryMoveNode.Parent == null)
                    {
                        throw new Exception($"{nameof(inMemoryMoveNode)} parent is null!" );
                    }
                    inMemoryMoveNode.Parent.RemoveChild(inMemoryMoveNode);
                }
            }
            else
            {
                throw new Exception($"Could not find new parent path in in memory node for node title {newParentTitle}"); // error in test setup
            }

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