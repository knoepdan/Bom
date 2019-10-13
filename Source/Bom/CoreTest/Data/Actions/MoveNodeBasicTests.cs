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
    public class MoveNodeBasicTests : IDisposable
    {
        public MoveNodeBasicTests()
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
            lock (DbLockers.DbLock)
            {
                EnsureSampleData(Context);

                // tests
                MoveLeaveUp();
                MoveNoneLeaveUp();
                MoveNoneLeaveUpAndMoveChildren();
                MoveNoneLeaveToAnotherBranch();

                // more tests would be possible but tests are more concise in when they are done differently
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
            var targetParent = RootNode.DescendantsAndI.Where(n => n.Level == 3 && n.Parent != node.Ancestors.First(a => a.Level == 2)).First();
            var args = new TestMoveNodeArgs(node, targetParent, false);
            var movedPath = TestMoveNodePath(args);
        }

        private Path TestMoveNodePath(TestMoveNodeArgs args)
        {
            // remember some state before
            TreeNode<SimpleNode>? oldParent = args.ToMoveNode.Parent;
            var siblings = args.ToMoveNode.Siblings;

            // do actual move
            var movedPath = MoveNodePath(args.ToMoveNode.Data.Title, args.NewParentNode?.Data?.Title, args.MoveChildrenToo);

            // ## test if parent really is the new parent
            var dbParentPath = this.Context.GetPaths().DirectParent(movedPath);
            if (dbParentPath == null || dbParentPath.Node == null)
            {
                throw new Exception($"{nameof(dbParentPath)} is null or its node is null! ({dbParentPath == null})");
            }
            if (dbParentPath.Node?.Title != args.NewParentNode?.Data?.Title)
            {
                throw new Exception($"Expected parentPath is not correct. Expected title: {args.NewParentNode?.Data?.Title}. Actual: {dbParentPath?.Node?.Title} / {dbParentPath?.NodePathString}");
            }

            // ## Test children of parent (must be the existing ones plus the moved one
            {
                var expected = new List<TreeNode<SimpleNode>>();
                if (args.NewParentNode != null)
                {
                    expected.AddRange(args.NewParentNode.Children);
                }
                var newChildren = this.Context.GetPaths().Descendants(dbParentPath, 1).ToList();
                CheckIfAreTheSameAndThrowIfNot(expected, newChildren, "checking children of parent");
            }

            // ## Test children of moved node
            {
                var newMovedChildren = this.Context.GetPaths().Descendants(movedPath, 1).ToList();
                if (args.MoveChildrenToo)
                {
                    CheckIfAreTheSameAndThrowIfNot(args.ToMoveNode.Children, newMovedChildren, "checking children of moved node");
                }
                else if (newMovedChildren.Count > 0)
                {
                    throw new Exception($"Moved path should not have any children as moving children was set to {args.MoveChildrenToo}, nof children {newMovedChildren.Count} ({string.Join(", ", newMovedChildren.Select(x => x.Node?.Title))}");
                }

            }
            // ## check all descendants of old parent
            {
                if (oldParent != null)
                {
                    var dbOldParentPath = this.Context.GetPaths().First(p => p.Node != null && p.Node.Title == oldParent.Data.Title);
                    var currentDescendants = this.Context.GetPaths().Descendants(dbOldParentPath, 9999).ToList(); // level so hight we get all children and subchildren .. we just want to check all children here
                    if (currentDescendants.Count != oldParent.Descendants.Count)
                    {
#if DEBUG
                        var dbRoot = this.Context.GetPaths().First(p => p.Node != null && p.Node.Title == "1a");
                        var dbAllNodes = this.Context.GetPaths().Descendants(dbRoot, 9999).ToList();
                        dbAllNodes.Add(dbRoot);
                        var inMemoryModel = TreeNodeUtils.CreateInMemoryModel(dbAllNodes);
#endif
                        // must be 2 less (the one that was moved and parent node itself may not be counted)
                        throw new Exception($"The number of descendants does not mach the expected number. Expected: {(args.ToMoveNode?.Parent?.Descendants.Count)}, actual: {currentDescendants.Count }");
                    }

                    // check descendants
                    CheckIfAreTheSameAndThrowIfNot(oldParent.Descendants, currentDescendants, "checking descendants of moved node");


                    // check direct children
                    foreach (var directChild in oldParent.Children)
                    {
                        if (!currentDescendants.Any(d => d.Node != null && d.Node.Title == directChild.Data.Title && d.Level == directChild.Level))
                        {
                            throw new Exception($"Direct children not ok. '{directChild.Data.Title}' with level {directChild.Level} not found in db data");
                        }
                    }
                }
            }
            return movedPath;
        }

        private void CheckIfAreTheSameAndThrowIfNot(IEnumerable<TreeNode<SimpleNode>> expected, IEnumerable<Path> actual, string additionalMsgInCaseOfError = "")
        {
            var expectedTitles = expected.Select(x => x.Data.Title).ToList();
            var actualTitles = actual.Where(x => x.Node != null).Select(x => x.Node?.Title != null ? x.Node.Title : "").ToList();
            ComparisonUtils.ThrowIfDuplicates(expectedTitles);
            ComparisonUtils.ThrowIfDuplicates(actualTitles);
            bool isResultAsExpected = ComparisonUtils.HasSameContent(actualTitles, expectedTitles); // possible improvment.. replace with simple foreach and also check level
            if (!isResultAsExpected)
            {
                throw new Exception($"Titles of moved node are not as expected! Expected: {string.Join(", ", expectedTitles)} | actual :  {string.Join(", ", actualTitles)} | {additionalMsgInCaseOfError}");
            }
        }

        private Path MoveNodePath(string moveTitle, string? newParentTitle, bool moveChildrenToo)
        {
            var moveNode = Context.GetPaths().First(x => x.Node != null && x.Node.Title == moveTitle);
            var newParentNode = Context.GetPaths().First(x => x.Node != null && x.Node.Title == newParentTitle);
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