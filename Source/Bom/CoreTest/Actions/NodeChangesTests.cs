using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Bom.Core.DataAccess;
using Bom.Core.Model;
using Bom.Core.Actions.Utils;

namespace Bom.Core.Actions
{
    public class NodeChangesTests : IDisposable
    {
        public NodeChangesTests()
        {
            this.Context = TestHelpers.GetModelContext(true);
            var dataFactory = new TestDataFactory();
            RootNode = dataFactory.CreateSampleNodes(MaxLevel, NofChildrenPerNode);
        }

        public const int MaxLevel = 4;

        public const int NofChildrenPerNode = 2;

        private Bom.Core.Data.ModelContext Context { get; }

        private TreeNode<MemoryNode> RootNode { get; }


        [Fact]
        public void Simple_move_works()
        {
            EnsureSampleData(Context);
            MoveLeaveUp();
        }

        private void MoveLeaveUp()
        {
            var leaveNode = RootNode.AllNodes.GetChildrenByAbsoluteLevel(MaxLevel).First();
            var targetParent = RootNode.AllNodes.GetChildrenByAbsoluteLevel(2).GetChildNodeByPos(2);
            var args = new TestMoveNodeArgs(leaveNode, targetParent, false);

            var movedPath = TestMoveNodePath(args);
        }

        private Path TestMoveNodePath(TestMoveNodeArgs args)
        {
            // Do actual move!
            var movedPath = MoveNodePath(args.ToMoveNode.Data.Title, args.NewParentNode.Data.Title, args.MoveChildrenToo);

            // ## test if parent really is the new parent
            var dbParentPath = this.Context.GetPaths().GetDirectParent(movedPath);
            if (dbParentPath.Node.Title != args.NewParentNode.Data.Title)
            {
                throw new Exception($"Expected parentPath is not correct. Expected title: {args.NewParentNode.Data.Title}. Actual: {dbParentPath.Node.Title} / {dbParentPath.NodePathString}");
            }

            // ## Test children of parent (must be the existing ones plus the moved one
            {
                var expected = new List<TreeNode<MemoryNode>>(args.NewParentNode.Children);
                expected.Add(args.ToMoveNode);

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
            // ## siblings of moved node not affected (todo)
            {
                if (args.ToMoveNode.Parent != null && args.ToMoveNode.Parent.Children.Count > 1)
                {
                    var dbOldParentPath = this.Context.GetPaths().First(p => p.Node.Title == args.ToMoveNode.Parent.Data.Title);
                    var currentChildren = this.Context.GetPaths().GetChildren(dbOldParentPath, 9999).ToList(); // level so hight we get all children and subchildren .. we just want to check all children here
                    if (currentChildren.Count != args.ToMoveNode.Parent.AllNodes.Count() - 2)
                    {
                        // must be 2 less (the one that was moved and parent node itself may not be counted)
                        throw new Exception($"The number of children does not mach the expected number. Expected: {(args.ToMoveNode.Parent.AllNodes.Count() - 2)}, actual: {currentChildren.Count }");
                    }

                    // check direct children
                    CheckIfAreTheSameAndThrowIfNot(args.ToMoveNode.Parent.Children.Where(c => c.Data.Title != args.ToMoveNode.Data.Title), currentChildren, "checking siblings of moved node");
                }
            }
            return movedPath;
        }

        private void CheckIfAreTheSameAndThrowIfNot(IEnumerable<TreeNode<MemoryNode>> expected, IEnumerable<Path> actual, string additionalMsgInCaseOfError = "")
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
            var movedPath = prov.MovePath(moveNode.PathId, newParentNode.PathId, moveChildrenToo);
            //this.Context.SaveChanges(); not necessary.. is saved because of stored procedure!
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