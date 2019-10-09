using System;
using Bom.Core.Model;
using Ch.Knomes.Struct;

namespace Bom.Core.TestUtils.Models
{
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