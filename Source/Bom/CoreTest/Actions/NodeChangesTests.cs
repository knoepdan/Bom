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
    public class NodeChangesTests : IDisposable
    {
        public NodeChangesTests()
        {
            this.Context = TestHelpers.GetModelContext(true);
            RootNode = TestDataFactory.CreateSampleNodes(MaxLevel, NofChildrenPerNode);
        }

        public const int MaxLevel = 5;

        public const int NofChildrenPerNode = 2;

        private Bom.Core.Data.ModelContext Context { get; set; }

        private TreeNode<SimpleNode> RootNode { get; }


        [Fact]
        public void Moving_path_works()
        {
            EnsureSampleData(Context);
           
            // tests
            MoveLeaveUp();
            MoveNoneLeaveUp();
            MoveNoneLeaveUpAndMoveChildren();
           
            // TODO test:  -> move leave to another branch (also as leave and not)
            MoveNoneLeaveToAnotherBranch(); // TODO with or without children
                                            // TODO test: Move non leave node up (with and without children) -> should work

            // TODO test: move node down to another branch (with or without children) -> should work  (its same as moving to another branch.. maybe test not necessary) 
            // TODO test: move node down with children: -> must fail
            // TODO test: move node down without children: -> should work
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
            // TODO
            //var leaveNode = RootNode.DescendantsAndI.GetChildrenByAbsoluteLevel(3).First(); // 1a-2a-3a
            //var targetParent = RootNode.DescendantsAndI.GetChildrenByAbsoluteLevel(1).First(); // 1a
            //var args = new TestMoveNodeArgs(leaveNode, targetParent, true);
            //var movedPath = TestMoveNodePath(args);
        }


        private Path TestMoveNodePath(TestMoveNodeArgs args)
        {
            // remember some state before
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

            // perform some other e
            if (args.MoveChildrenToo )
            {
                CheckIfAreTheSameAndThrowIfNot(args.ToMoveNode.Children, childrenBeforeMoving);
            }
            else if (!args.MoveChildrenToo && args.ToMoveNode.Children.Count > 0)
            {
                throw new Exception($"InMemory moved node has  {args.ToMoveNode.Children.Count} children but shoulde have none "); // check that both are in same tree
            }

            // compare with DB node (not possible to compare string representation as order of children not the same)
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
            var inMemoryNewParentNode = this.RootNode.DescendantsAndI.First(n => n.Data.Title == newParentTitle);
            inMemoryMoveNode.MoveToNewParent(inMemoryNewParentNode, moveChildrenToo);

            // new context otherwise we might get wrong data (important)
            this.Context.Dispose();
            this.Context = TestHelpers.GetModelContext(true);

            return movedPath;
        }

        // private Path Get


        //[Fact]
        //public void Delete_root_with_children_will_throw()
        //{
        //    try
        //    {
        //        var root = EnsureSampleData(Context);
        //        var prov = new PathNodeProvider(Context);
        //        prov.DeletePath(root.PathId, true, null);
        //        Assert.True(false); // make it fail
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Expected error happened:" + ex);
        //    }
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