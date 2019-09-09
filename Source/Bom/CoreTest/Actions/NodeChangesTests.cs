using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Bom.Core.DataAccess;
using Bom.Core.Model;
using Bom.Core.Actions.Utils;
using Bom.Core.Utils;
using Ch.Knomes.Structure;

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

        private Bom.Core.Data.ModelContext Context { get; }

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
            var leaveNode = RootNode.DescendantsAndI.GetChildrenByAbsoluteLevel(MaxLevel).First(); // 1a-2a-3a-4a-5a
            var targetParent = RootNode.DescendantsAndI.GetChildrenByAbsoluteLevel(2).GetChildNodeByPos(2);// 1a-2b
            var args = new TestMoveNodeArgs(leaveNode, targetParent, false);

            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNoneLeaveUp()
        {
            var node = RootNode.DescendantsAndI.GetChildrenByAbsoluteLevel(3).First(); // 1a-2a-3a
            var targetParent = RootNode.DescendantsAndI.GetChildrenByAbsoluteLevel(1).First(); // 1a
            var args = new TestMoveNodeArgs(node, targetParent, false);
            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNoneLeaveUpAndMoveChildren()
        {
            var baseNode = RootNode.Children.Skip(1).First();// 1a-2b
            var node = baseNode.DescendantsAndI.GetChildrenByAbsoluteLevel(3).First(); // 1a-2a-3a
            var targetParent = baseNode.DescendantsAndI.GetChildrenByAbsoluteLevel(1).First(); // 1a
            var args = new TestMoveNodeArgs(node, targetParent, true);
            var movedPath = TestMoveNodePath(args);
        }

        private void MoveNoneLeaveToAnotherBranch()
        {
            var leaveNode = RootNode.DescendantsAndI.GetChildrenByAbsoluteLevel(3).First(); // 1a-2a-3a
            var targetParent = RootNode.DescendantsAndI.GetChildrenByAbsoluteLevel(1).First(); // 1a
            var args = new TestMoveNodeArgs(leaveNode, targetParent, true);
            var movedPath = TestMoveNodePath(args);
        }


        private Path TestMoveNodePath(TestMoveNodeArgs args)
        {
            // remember some state before
            TreeNode<SimpleNode> oldParent = args.ToMoveNode.Parent;
            var siblings = oldParent.Siblings;

            // do actual move
            var movedPath = MoveNodePath(args.ToMoveNode.Data.Title, args.NewParentNode.Data.Title, args.MoveChildrenToo);

            // ## test if parent really is the new parent
            var dbParentPath = this.Context.GetPaths().GetDirectParent(movedPath);
            if (dbParentPath.Node.Title != args.NewParentNode.Data.Title)
            {
                throw new Exception($"Expected parentPath is not correct. Expected title: {args.NewParentNode.Data.Title}. Actual: {dbParentPath.Node.Title} / {dbParentPath.NodePathString}");
            }

            // ## Test children of parent (must be the existing ones plus the moved one
            {
                var expected = new List<TreeNode<SimpleNode>>(args.NewParentNode.Children);
                var newChildren = this.Context.GetPaths().GetChildren(dbParentPath, 1);
                CheckIfAreTheSameAndThrowIfNot(expected, newChildren, "checking children of parent");
            }

            // ## Test children of moved node
            {
                var newMovedChildren = this.Context.GetPaths().GetChildren(movedPath, 1).ToList();
                if (args.MoveChildrenToo)
                {
                    CheckIfAreTheSameAndThrowIfNot(args.ToMoveNode.Children, newMovedChildren, "checking children of moved node");
                }
                else if (newMovedChildren.Count > 0)
                {
                    throw new Exception($"Moved path should not have any children as moving children was set to {args.MoveChildrenToo}, nof children {newMovedChildren.Count} ({string.Join(", ", newMovedChildren.Select(x => x.Node.Title))}");
                }

            }
            // ## siblings of moved node not affected
            {
                if (oldParent != null)
                {
                    var dbOldParentPath = this.Context.GetPaths().First(p => p.Node.Title == oldParent.Data.Title);
                    var currentChildren = this.Context.GetPaths().GetChildren(dbOldParentPath, 9999).ToList(); // level so hight we get all children and subchildren .. we just want to check all children here
                    if (currentChildren.Count != oldParent.Descendants.Count)
                    {
                        // must be 2 less (the one that was moved and parent node itself may not be counted)
                        throw new Exception($"The number of children does not mach the expected number. Expected: {(args.ToMoveNode.Parent.DescendantsAndI.Count() - 2)}, actual: {currentChildren.Count }");
                    }

                    // check direct children
                    CheckIfAreTheSameAndThrowIfNot(oldParent.Children.Where(c => c.Data.Title != args.ToMoveNode.Data.Title), currentChildren, "checking siblings of moved node");
                }
            }
            return movedPath;
        }

        private void CheckIfAreTheSameAndThrowIfNot(IEnumerable<TreeNode<SimpleNode>> expected, IEnumerable<Path> actual, string additionalMsgInCaseOfError = "")
        {
            var expectedTitles = expected.Select(x => x.Data.Title).ToList();
            var actualTitles = actual.Select(x => x.Node.Title).ToList();
            ComparisonUtils.ThrowIfDuplicates(expectedTitles);
            ComparisonUtils.ThrowIfDuplicates(actualTitles);
            bool isResultAsExpected = ComparisonUtils.HasSameContent(actualTitles, expectedTitles);
            if (!isResultAsExpected)
            {
                throw new Exception($"Sibling titles of moved node are not as expected! Expected: {string.Join(", ", expectedTitles)} | actual :  {string.Join(", ", actualTitles)} | {additionalMsgInCaseOfError}");
            }
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