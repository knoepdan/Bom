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
using Ch.Knomes.Struct;

namespace Bom.Core.Actions.Utils
{
    public class TestDeleteNodeArgs
    {
        public TestDeleteNodeArgs(TreeNode<SimpleNode> deleteNode, bool alsoDeleteNode = false, bool deleteChildrenToo = false)
        {
            ToDeleteNode = deleteNode;
            AlsoDeleteNode = alsoDeleteNode;
            DeleteChildrenToo = deleteChildrenToo;
        }

        public bool AlsoDeleteNode { get; set; }

        public bool DeleteChildrenToo { get; set; }

        public TreeNode<SimpleNode> ToDeleteNode { get; set; }
    }
}