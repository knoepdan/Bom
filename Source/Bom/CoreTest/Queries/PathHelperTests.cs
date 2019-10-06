using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Bom.Core.Data;
using Bom.Core.Model;
using Bom.Core.Actions.Utils;
using Bom.Core.Utils;
using Ch.Knomes.Struct;
using Ch.Knomes.Struct.Testing;
using Bom.Core.TestUtils.Models;

namespace Bom.Core.Queries
{
    public class PathHelperTests
    {
        public PathHelperTests()
        {
            MemoryRoot = TestDataFactory.CreateSampleAnimalNodes();
            Paths = CreateAllPaths(MemoryRoot);
        }

        private TreeNode<SimpleNode> MemoryRoot { get; }

        private IList<Path> Paths { get; }

        private Path RootPath => Paths.OrderBy(x => x.Level).First();

        private Path LeavePath => Paths.OrderByDescending(x => x.Level).First();

        private Path SomePath => Paths.OrderBy(x => x.Level).First(x => x.Level == 2 && Paths.Any(p => p.NodePathString.StartsWith(x.NodePathString, StringComparison.InvariantCulture) && p.Level >= 4));

        [Fact]
        public void GetParentPathFragments_work()
        {
            // root is always empty
            Assert.True(PathHelper.GetParentPathFragments(RootPath, 0).Count() == 0);
            Assert.True(PathHelper.GetParentPathFragments(RootPath, 5).Count() == 0);

            // leave
            var memLeave = GetMemoryPath(this.LeavePath);
            var pathsFrag = PathHelper.GetParentPathFragments(LeavePath, 999).ToList();
            var memPathFrag = CreatePathFragsFromMemoryNode(memLeave, 999).Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(pathsFrag, memPathFrag));
            var pathsFrag2 = PathHelper.GetParentPathFragments(LeavePath, 2).ToList();
            var memPathFrag2 = CreatePathFragsFromMemoryNode(memLeave, 2).Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(pathsFrag2, memPathFrag2));
        }



        [Fact]
        public void XXX_reminder()
        {
            // GetParentPathFragments


            //GetAllParentPaths

            // GetParentPath

            // GetNodeIdsFromPath

            // IsRoot
        }

        private TreeNode<SimpleNode> GetMemoryPath(Path path)
        {
            return this.MemoryRoot.DescendantsAndI.First(x => x.Data.Id == path.NodeId);
        }

        private static List<int> CreatePathFragsFromMemoryNode(TreeNode<SimpleNode> memNode, int stepsUp)
        {
            var nodeIds = new List<int>();
            var parent = memNode.Parent;
            int steps = 0;
            while (parent != null && steps < stepsUp)
            {
                nodeIds.Add(parent.Data.Id);
                parent = parent.Parent;
                steps++;
            }
            nodeIds.Reverse();
            return nodeIds;
        }

        private static string CreatePathStringFromMemoryNode(TreeNode<SimpleNode> memNode, int stepsUp)
        {
            var nodeIds = CreatePathFragsFromMemoryNode(memNode, stepsUp);
            var pathString = CreatePathString(nodeIds);
            return pathString;
        }

        private static string CreatePathString(IEnumerable<int> nodeIds)
        {
            if(nodeIds.Count() == 0)
            {
                throw new ArgumentException("passed nodeIds may not be empty", nameof(nodeIds));
            }
            var sep = Path.Separator;
            var pathString = sep + string.Join(sep, nodeIds) + nodeIds;
            return pathString;
        }

        private static IList<Path> CreateAllPaths(TreeNode<SimpleNode> root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }
            var paths = new List<Path>();
            foreach (var memNode in root.DescendantsAndI)
            {
                var path = new TestPath(memNode); // all ids/nodepathis should be set correctly
                paths.Add(path);
            }
            return paths;
        }
    }
}