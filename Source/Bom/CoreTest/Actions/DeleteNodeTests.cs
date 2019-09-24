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
        //    AnimalRootNode = TestDataFactory.CreateSampleAnimalNodes();
        }

        public const int MaxLevel = 5;

        public const int NofChildrenPerNode = 2;

        private Bom.Core.Data.ModelContext Context { get; set; }

        private TreeNode<SimpleNode> RootNode { get; }

  //      private TreeNode<SimpleNode> AnimalRootNode { get; }


        [Fact]
        public void Deleteing_path_works()
        {
            EnsureSampleData(Context, RootNode, true);
          

            // final test
            this.Context.Dispose();
            this.Context = TestHelpers.GetModelContext(true);
            var rootNodes = new List<TreeNode<SimpleNode>>();
            rootNodes.Add(this.RootNode.Root);
            //     rootNodes.Add(this.AnimalRootNode.Root);
            //     rootNodes.Add(createdRoot);
            //     rootNodes.Add(anotherRootNode);
            this.CompareAllInMemoryAndAllDbNodes(rootNodes); // method handles duplicates
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
                var inMemoryRoot = memRoots.FirstOrDefault(x => x.Data.Title == dbRoot.Data.Node.Title);
                if (inMemoryRoot == null)
                {
                    throw new Exception("Could not find in memory root with title: " + dbRoot.Data.Node.Title);
                }
                bool areOrigNodesEqual = inMemoryRoot.AreDescendantsAndIEqual(dbRoot, (node, simpleNode) => { return node.Data.Title == simpleNode.Data.Node.Title; });
                if (!areOrigNodesEqual)
                {
                    throw new Exception($"Trees are not equal (Root: {inMemoryRoot.Data.Title})");
                }
            }
        }


        private void DeleteNodePath(TestDeleteNodeArgs args)
        {
            var deleteNode = Context.GetPaths().First(x => x.Node.Title == args.ToDeleteNode.Data.Title);
            int? newNodeMainPathId = null;
            if (!string.IsNullOrEmpty(args.NewMainNodeTitle))
            {
                var newMainPath = Context.GetPaths().First(x => x.Node.Title == args.NewMainNodeTitle);
                newNodeMainPathId = newMainPath.PathId;
            }
            var prov = new PathNodeProvider(Context);
            prov.DeletePath(deleteNode, args.AlsoDeleteNode, newNodeMainPathId);
            //this.Context.SaveChanges(); not necessary.. is saved because of stored procedure!
            System.Diagnostics.Debug.WriteLine($"Node {args.ToDeleteNode.Data.Title} was deleted   (also deleteNode: {args.AlsoDeleteNode} new mainPath: {args.NewMainNodeTitle})");

            // keep in memory construct in sync
            var inMemoryDeleteNode = args.ToDeleteNode;
            inMemoryDeleteNode.ExtractNodeFromTree();

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