using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Testing;
using Bom.Core.Data;
using Bom.Core.Model;
using Bom.Core.Utils;
using Ch.Knomes.Structure;

namespace Bom.Core.Actions.Utils
{
    public class TestDeleteNodeArgs
    {
        public TestDeleteNodeArgs(TreeNode<SimpleNode> deleteNode, bool alsoDeleteNode, string newMainNodeTitle)
        {
            AlsoDeleteNode = alsoDeleteNode;
            AlsoDeleteNode = alsoDeleteNode;
            NewMainNodeTitle = newMainNodeTitle;
        }

        public TestDeleteNodeArgs(TreeNode<SimpleNode> deleteNode, bool alsoDeleteNode, TreeNode<SimpleNode> newMainNode)
        {
            AlsoDeleteNode = alsoDeleteNode;
            AlsoDeleteNode = alsoDeleteNode;
            if (newMainNode != null)
            {
                NewMainNodeTitle = newMainNode.Data.Title;
            }
        }

        public bool AlsoDeleteNode { get; set; }

        public TreeNode<SimpleNode> ToDeleteNode { get; set; }

        public string NewMainNodeTitle { get; set; }
    }
}