using System;
using System.Text;
using Bom.Core.Model;
using Ch.Knomes.Struct;

namespace Bom.Core.TestUtils.Models
{
    public class TestPath : Path
    {
        private static int idCounter = 10000;

        public TestPath(string customNodePathString) : this(new TreeNode<SimpleNode>(new SimpleNode("")))
        {
            this.SetNodePath(customNodePathString);
        }

        public TestPath(TreeNode<SimpleNode> memNode)
        {
            if (memNode == null)
            {
                throw new ArgumentNullException(nameof(memNode));
            }
            System.Threading.Interlocked.Increment(ref idCounter);
            this.PathId = idCounter;
            this.Level = (short)memNode.Level;
            this.MemNode = memNode;

            this.Node = new TestNode(this);
            this.NodeId = this.Node.NodeId;

            // now caluclate node path
            const char sep = Path.Separator;
            var sb = new StringBuilder(sep);
            var parents = this.MemNode.Ancestors;
            parents.Reverse();
            foreach (var p in parents)
            {
                sb.Append(p.Data.Id);
                sb.Append(sep);
            }
            sb.Append(this.NodeId);
            sb.Append(sep);
            this.NodePathString = sb.ToString();
        }

        public TreeNode<SimpleNode> MemNode { get; }

        public void SetNodePath(string customNodePath, bool ensureStartEnd = true)
        {
            if (customNodePath == null)
            {
                customNodePath = "";
            }
            if (ensureStartEnd)
            {
                customNodePath = Path.Separator + customNodePath.Trim().Trim(Path.Separator) + Path.Separator;
            }
            this.NodePathString = customNodePath;
            this.Level = (short)(NodePathString.Length - NodePathString.Replace("" + Path.Separator, "", StringComparison.InvariantCulture).Length - 1);
        }
    }
}