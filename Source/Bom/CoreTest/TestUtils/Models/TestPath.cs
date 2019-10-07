using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Bom.Core.Data;
using Bom.Core.Model;
using Ch.Knomes.Struct;

namespace Bom.Core.TestUtils.Models
{
    public class TestPath : Path
    {
        private static int idCounter = 10000;

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
            foreach(var p in parents)
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
            if(customNodePath == null)
            {
                customNodePath = "";
            }
            if (ensureStartEnd)
            {
                customNodePath = Path.Separator + customNodePath.Trim().Trim(Path.Separator) + Path.Separator;
            }
            this.NodePathString = customNodePath;
            this.Level = (short)(NodePathString.Length - NodePathString.Replace(""+Path.Separator, "", StringComparison.InvariantCulture).Length -1);
        }
    }
    public class TestNode : Node
    {
        public TestNode(TestPath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.NodeId = path.MemNode.Data.Id;
            this.Title = path.MemNode.Data.Title;
            this.MainPath = path;
            this.MainPathId = path.PathId;
            this.MemNode = path.MemNode;
        }

        public TreeNode<SimpleNode> MemNode { get; }
    }
}