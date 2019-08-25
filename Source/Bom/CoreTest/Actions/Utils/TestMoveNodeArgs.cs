using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Bom.Core.DataAccess;
using Bom.Core.Model;


namespace Bom.Core.Actions.Utils
{
    public class TestMoveNodeArgs
    {
        public TestMoveNodeArgs(TreeNode<MemoryNode> moveNode, TreeNode<MemoryNode> newParentNode, bool moveChildrenToo = false)
        {
            ToMoveNode = moveNode;
            NewParentNode = newParentNode;
            MoveChildrenToo = moveChildrenToo;
        }

        public bool MoveChildrenToo { get; set; }

        public TreeNode<MemoryNode> ToMoveNode { get; set; }

        public TreeNode<MemoryNode> NewParentNode { get; set; }

    }
}
