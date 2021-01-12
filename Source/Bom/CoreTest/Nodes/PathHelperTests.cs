using System;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Common;
using Bom.Core.Nodes;
using Bom.Core.Nodes.DbModels;
using Ch.Knomes.Struct;
using Bom.Core.TestUtils.Models;

namespace Bom.Core.Data
{
    public class PathHelperTests
    {
        [Fact]
        public void GetParentPathFragments_work()
        {
            var tmpPath = new TestPath(string.Join(Path.Separator, new[] { "aa", "bb", "cc", "dd" }));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] { "aa", "bb", "cc", "dd" }, PathHelper.GetParentPathFragments(tmpPath, 0)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] { "aa", "bb", "cc" }, PathHelper.GetParentPathFragments(tmpPath, 1)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] { "aa", "bb" }, PathHelper.GetParentPathFragments(tmpPath, 2)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] { "aa", }, PathHelper.GetParentPathFragments(tmpPath, 3)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] { "aa", }, PathHelper.GetParentPathFragments(tmpPath, 4)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] { "aa", }, PathHelper.GetParentPathFragments(tmpPath, 5)));

            // root is always empty
            var tmpRootPath = new TestPath(new TreeNode<SimpleNode>(new SimpleNode("")));
            tmpPath.SetNodePath("aa", true);
            Assert.True(PathHelper.GetParentPathFragments(tmpRootPath, 0).Count() == 0);
            Assert.True(PathHelper.GetParentPathFragments(tmpRootPath, 5).Count() == 0);
        }

        [Fact]
        public void GetAllParentPaths_works()
        {
            var tmpPath = new TestPath(string.Join(Path.Separator, new[] { "aa", "bb", "cc", "dd" }));
            Assert.True( PathHelper.GetAllParentPaths(tmpPath, 0).Count() == 0);
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] {"/aa/bb/cc/" }, PathHelper.GetAllParentPaths(tmpPath, 1)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] { "/aa/bb/cc/", "/aa/bb/" }, PathHelper.GetAllParentPaths(tmpPath, 2)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] { "/aa/bb/cc/", "/aa/bb/", "/aa/" }, PathHelper.GetAllParentPaths(tmpPath, 3)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] { "/aa/bb/cc/", "/aa/bb/", "/aa/" }, PathHelper.GetAllParentPaths(tmpPath, 4)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new[] { "/aa/bb/cc/", "/aa/bb/", "/aa/" }, PathHelper.GetAllParentPaths(tmpPath, 5)));

            // root
            var tmpRootPath = new TestPath(new TreeNode<SimpleNode>(new SimpleNode("")));
            tmpPath.SetNodePath("aa", true);
            Assert.True(PathHelper.GetAllParentPaths(tmpRootPath, 0).Count() == 0);
            Assert.True(PathHelper.GetAllParentPaths(tmpRootPath, 5).Count() == 0);
        }

        [Fact]
        public void GetParentPath_works()
        {
            var tmpPath = new TestPath(string.Join(Path.Separator, new[] { "aa", "bb", "cc", "dd" }));
            Assert.Equal("/aa/bb/cc/dd/", PathHelper.GetParentPath(tmpPath, 0));
            Assert.Equal( "/aa/bb/cc/" , PathHelper.GetParentPath(tmpPath, 1));
            Assert.Equal("/aa/bb/", PathHelper.GetParentPath(tmpPath, 2));
            Assert.Equal("/aa/", PathHelper.GetParentPath(tmpPath, 3));
            Assert.Equal("/aa/", PathHelper.GetParentPath(tmpPath, 4));
            Assert.Equal("/aa/", PathHelper.GetParentPath(tmpPath, 5));

            // root
            var tmpRootPath = new TestPath("aa");
            Assert.Equal("", PathHelper.GetParentPath(tmpRootPath, 0));
            Assert.Equal("", PathHelper.GetParentPath(tmpRootPath, 5));
        }

        [Fact]
        public void GetNodeIdsFromPath_works()
        {
            var sep = Path.Separator;
            Assert.True(PathHelper.GetNodeIdsFromPath("").Count() == 0);
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new long[] { 1 }, PathHelper.GetNodeIdsFromPath("1")));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new long[] { 70 },PathHelper.GetNodeIdsFromPath(sep+"70" + sep)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new long[] { 83 }, PathHelper.GetNodeIdsFromPath(sep + "83")));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new long[] { 321 }, PathHelper.GetNodeIdsFromPath( "321" + sep)));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new long[] { 1, 7, 999, 44 }, PathHelper.GetNodeIdsFromPath(string.Join(sep, new[] { "1", "7", "999", "44" }))));
            Assert.True(ComparisonUtils.HasSameContentInSameOrder(new long[] { 66, 46}, PathHelper.GetNodeIdsFromPath(string.Join(sep, new[] { "66", "46" }))));
            
            // some negative tests
            Assert.False(ComparisonUtils.HasSameContentInSameOrder(new long[] { 46, 66 }, PathHelper.GetNodeIdsFromPath(string.Join(sep, new[] { "66", "46" })))); // wrong order
            Assert.False(ComparisonUtils.HasSameContentInSameOrder(new long[] { 66, 77 }, PathHelper.GetNodeIdsFromPath(string.Join(sep, new[] { "66", "46" })))); // one wrong number

            // ex tests
            Assert.Throws<FormatException>(() => { PathHelper.GetNodeIdsFromPath(string.Join(sep, new[] { "lkjlkj", "46" })).ToList(); });
        }


        [Fact]
        public void IsRoot_works()
        {
            var tmpPath = new TestPath(string.Join(Path.Separator, new[] { "aa", "bb", "cc", "dd" }));
            Assert.False(tmpPath.Level == 1);
            Assert.False(tmpPath.IsRoot());

            var tmpRootPath = new TestPath("aa");
            Assert.True(tmpRootPath.Level == 1);
            Assert.True(tmpRootPath.IsRoot());
        }

        #region outcommented stuff as not needed

        //public PathHelperTests()
        //{
        //    MemoryRoot = TestDataFactory.CreateSampleAnimalNodes();
        //    Paths = CreateAllPaths(MemoryRoot);
        //}

        //private TreeNode<SimpleNode> MemoryRoot { get; }

        //private IList<TestPath> Paths { get; }

        //private TestPath RootPath => Paths.OrderBy(x => x.Level).First();

        //private TestPath LeavePath => Paths.OrderByDescending(x => x.Level).First();

        //private TestPath SomePath => Paths.OrderBy(x => x.Level).First(x => x.Level == 2 && Paths.Any(p => p.NodePathString.StartsWith(x.NodePathString, StringComparison.InvariantCulture) && p.Level >= 4));


        //private TreeNode<SimpleNode> GetMemoryPath(Path path)
        //{
        //    return this.MemoryRoot.DescendantsAndI.First(x => x.Data.Id == path.NodeId);
        //}

        //private static List<int> CreatePathFragsFromMemoryNode(TreeNode<SimpleNode> memNode, int stepsUp)
        //{
        //    var nodeIds = new List<int>();
        //    var parent = memNode.Parent;
        //    int steps = 0;
        //    while (parent != null && steps < stepsUp)
        //    {
        //        nodeIds.Add(parent.Data.Id);
        //        parent = parent.Parent;
        //        steps++;
        //    }
        //    nodeIds.Reverse();
        //    return nodeIds;
        //}

        //private static string CreatePathStringFromMemoryNode(TreeNode<SimpleNode> memNode, int stepsUp)
        //{
        //    var nodeIds = CreatePathFragsFromMemoryNode(memNode, stepsUp);
        //    var pathString = CreatePathString(nodeIds);
        //    return pathString;
        //}

        //private static string CreatePathString(IEnumerable<int> nodeIds)
        //{
        //    if(nodeIds.Count() == 0)
        //    {
        //        throw new ArgumentException("passed nodeIds may not be empty", nameof(nodeIds));
        //    }
        //    var sep = Path.Separator;
        //    var pathString = sep + string.Join(sep, nodeIds) + nodeIds;
        //    return pathString;
        //}

        //private static IList<TestPath> CreateAllPaths(TreeNode<SimpleNode> root)
        //{
        //    if (root == null)
        //    {
        //        throw new ArgumentNullException(nameof(root));
        //    }
        //    var paths = new List<TestPath>();
        //    foreach (var memNode in root.DescendantsAndI)
        //    {
        //        var path = new TestPath(memNode); // all ids/nodepathis should be set correctly
        //        paths.Add(path);
        //    }
        //    return paths;
        //}

        #endregion
    }
}