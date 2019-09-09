using System;
using System.Collections.Generic;
using System.Text;
using Ch.Knomes.Structure;

namespace Ch.Knomes.Structure.Testing
{
    public static class TestDataFactory
    {
        /*
         *                          1a
         *          1a-2a                     1a-2b        
         *    1a-2a-3a   1a-2a-3b        1a-2b-3a   1a-2b-3b
         *    etc... 
         */
        public const string PositionChars = "abcdefghijklmnopqrstuvwxyz";

        private static char GetCharForPos(int pos)
        {
            if (pos > PositionChars.Length - 1)
            {
                throw new ArgumentException($"pos is to big. Max lenght is: {PositionChars.Length - 1}", nameof(pos));
            }
            return PositionChars[pos];
        }

        private static string GetTitle(TreeNode<SimpleNode> parentNode, int level, int pos)
        {
            var title = $"{parentNode.Data.Title}-{level}{GetCharForPos(pos)}";
            return title;
        }

        public static TreeNode<SimpleNode> CreateSampleNodes(int nofLevels, int nofPos = 3)
        {
            var root = new TreeNode<SimpleNode>(new SimpleNode("1a"));
            AddSampleNodesRecursive(root, nofLevels, nofPos);
            return root;
        }

        public static void AddSampleNodesRecursive(TreeNode<SimpleNode> parent, int nofLevels, int nofPos)
        {
            int level = parent.GetLevel();
            for (int pos = 0; pos < nofPos; pos++)
            {
                var nodeTitle = GetTitle(parent, (level+1), pos);
                var memNode = new SimpleNode(nodeTitle);
                var node = parent.AddChild(memNode);
                if (node.Level < nofLevels)
                {
                    AddSampleNodesRecursive(node, nofLevels, nofPos);
                }
            }
        }

        public static TreeNode<SimpleNode> CreateSampleNodesUnevenExample()
        {
            var root = new TreeNode<SimpleNode>(new SimpleNode("a"));
            {
                var node0 = root.AddChild(new SimpleNode("a-b1"));
                {
                    var node01 = node0.AddChild(new SimpleNode("a-b1-c1"));
                    {
                        var node010 = node0.AddChild(new SimpleNode("a-b1-c1-d1"));
                        {
                            var node0100 = node0.AddChild(new SimpleNode("a-b1-c1-d1-e1"));
                            var node0101 = node0.AddChild(new SimpleNode("a-b1-c1-d1-e2"));
                            var node0102 = node0.AddChild(new SimpleNode("a-b1-c1-d1-e3"));
                        }
                    }
                }

                var node1 = root.AddChild(new SimpleNode("a-b2"));
                {
                    var node10 = node1.AddChild(new SimpleNode("a-b2-c1"));
                    var node11 = node1.AddChild(new SimpleNode("a-b2-c2"));
                }
                var node2 = root.AddChild(new SimpleNode("a-b3"));
                {
                    var node20 = node2.AddChild(new SimpleNode("a-b3-c1"));
                    var node21 = node2.AddChild(new SimpleNode("a-b3-c2"));
                    var node22 = node2.AddChild(new SimpleNode("a-b3-c3"));
                    var node23 = node2.AddChild(new SimpleNode("a-b3-c4"));
                    {
                        var node230 = node2.AddChild(new SimpleNode("a-b3-c4-d1"));
                        var node231 = node2.AddChild(new SimpleNode("a-b3-c4-d2"));
                        var node232 = node2.AddChild(new SimpleNode("a-b3-c4-d3"));
                    }
                }
                var node3 = node2.AddChild(new SimpleNode("a-b4"));
            }
            return root;
        }


        public static TreeNode<SimpleNode> CreateSampleAnimalNodes()
        {
            var root = new TreeNode<SimpleNode>(new SimpleNode("Animals"));
            {
                var node0 = root.AddChild(new SimpleNode("Mammals"));
                {
                    var node01 = node0.AddChild(new SimpleNode("Xenarthra"));
                    {
                        var node010 = node0.AddChild(new SimpleNode("Pilosa"));
                        {
                            var node0100 = node0.AddChild(new SimpleNode("Anteater"));
                            var node0101 = node0.AddChild(new SimpleNode("Three-toed sloths"));
                            var node0102 = node0.AddChild(new SimpleNode("Twoe-toed sloths"));
                        }
                    }
                }

                var node1 = root.AddChild(new SimpleNode("Birds"));
                {
                    var node10 = node1.AddChild(new SimpleNode("Songbirds"));  // not fully correct, just data
                    var node11 = node1.AddChild(new SimpleNode("Non-Songbirds")); // not correct.. just data
                }
                var node2 = root.AddChild(new SimpleNode("Reptiles"));
                {
                    var node20 = node2.AddChild(new SimpleNode("Turtles"));
                    var node21 = node2.AddChild(new SimpleNode("Lizards")); // not fully true as snakes are not that separate
                    var node22 = node2.AddChild(new SimpleNode("Snakes"));
                    var node23 = node2.AddChild(new SimpleNode("Crocodilians"));
                    {
                        var node230 = node2.AddChild(new SimpleNode("Alligators"));
                        var node231 = node2.AddChild(new SimpleNode("Crocodiles"));
                        var node232 = node2.AddChild(new SimpleNode("Gavial"));
                    }
                }
                var node3 = node2.AddChild(new SimpleNode("Amphibians"));
            }
            return root;
        }
    }
}