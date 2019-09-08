using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Bom.Core.DataAccess;
using Bom.Core.Model;
using Bom.Core.Utils;


namespace Bom.Core.Actions.Utils
{
    public class TestMoveNodeArgs
    {
        public TestMoveNodeArgs(TreeNode<SimpleNode> moveNode, TreeNode<SimpleNode> newParentNode, bool moveChildrenToo = false)
        {
            ToMoveNode = moveNode;
            NewParentNode = newParentNode;
            MoveChildrenToo = moveChildrenToo;
        }

        public bool MoveChildrenToo { get; set; }

        public TreeNode<SimpleNode> ToMoveNode { get; set; }

        public TreeNode<SimpleNode> NewParentNode { get; set; }

    }
}
