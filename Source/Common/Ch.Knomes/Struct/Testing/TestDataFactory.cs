using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Ch.Knomes.Struct.Testing
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

        public static TreeNode<SimpleNode> CreateSampleNodes(int nofLevels, int nofPos = 3, string? prefix = "")
        {
            var root = new TreeNode<SimpleNode>(new SimpleNode("1a" + prefix));
            AddSampleNodesRecursive(root, nofLevels, nofPos, prefix);
            return root;
        }

        public static void AddSampleNodesRecursive(TreeNode<SimpleNode> parent, int nofLevels, int nofPos, string? prefix)
        {
            if(parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }
            int level = parent.GetLevel();
            for (int pos = 0; pos < nofPos; pos++)
            {
                var nodeTitle = GetTitle(parent, (level + 1), pos) + prefix;
                var memNode = new SimpleNode(nodeTitle);
                var node = parent.AddChild(memNode);
                if (node.Level < nofLevels)
                {
                    AddSampleNodesRecursive(node, nofLevels, nofPos, prefix);
                }
            }
        }

        public static TreeNode<SimpleNode> CreateSampleAnimalNodes()
        {
            var root = new TreeNode<SimpleNode>(new SimpleNode("Animals"));
            {
                var node0 = root.AddChild(new SimpleNode("Mammals"));
                {
                    var node01 = node0.AddChild(new SimpleNode("Xenarthra"));
                    {
                        var node010 = node01.AddChild(new SimpleNode("Pilosa"));
                        {
                            var node0100 = node010.AddChild(new SimpleNode("Anteater"));
                            var node0101 = node010.AddChild(new SimpleNode("Three-toed sloths"));
                            var node0102 = node010.AddChild(new SimpleNode("Twoe-toed sloths"));
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
                        var node230 = node23.AddChild(new SimpleNode("Alligators"));
                        var node231 = node23.AddChild(new SimpleNode("Crocodiles"));
                        {
                            var node2310 = node231.AddChild(new SimpleNode("saltwater crocodile"));
                            var node2311 = node231.AddChild(new SimpleNode("nile crocodile"));
                            var node2312 = node231.AddChild(new SimpleNode("orinoco crocodile"));
                            var node2313 = node231.AddChild(new SimpleNode("american crocodile"));
                        }
                        var node232 = node23.AddChild(new SimpleNode("Gavial"));
                    }
                }
                var node3 = node2.AddChild(new SimpleNode("Amphibians"));
                {
                    var node32 = node3.AddChild(new SimpleNode("Frogs and Toads"));
                    {
                        var node320 = node32.AddChild(new SimpleNode("sugar cane toad"));
                        var node321 = node32.AddChild(new SimpleNode("grasfrog"));
                    }
                    var node30 = node3.AddChild(new SimpleNode("Caecilian"));
                    var node31 = node3.AddChild(new SimpleNode("Salamanders"));
                }
            }
            return root;
        }

        #region TreeNode<SimpleNode> extensions 

        private static string GetNonParentPart(this TreeNode<SimpleNode> node)
        {
            if (!string.IsNullOrEmpty(node.Data.Title))
            {
                var allParts = node.Data.Title.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (allParts.Length > 0)
                {
                    // parent title is all without the last
                    var lastPart = allParts.Last();
                    return lastPart;
                }
                return node.Data.Title; // no parent part.. root
            }
            return "";
        }

        private static int GetLevel(this TreeNode<SimpleNode> node)
        {
            var definingPart = GetNonParentPart(node);
            if (!string.IsNullOrEmpty(definingPart))
            {
                var sb = new StringBuilder();
                foreach (var c in definingPart)
                {
                    if (char.IsDigit(c))
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        break;
                    }
                }
                var levelPart = sb.ToString();
                var level = int.Parse(levelPart, System.Globalization.CultureInfo.InvariantCulture); // will throw if does not comply to convention
                return level;
            }
            return 0;
        }
        #endregion

        #region outcommented

        //public static TreeNode<SimpleNode> CreateSampleNodesUnevenExample()
        //{
        //    var root = new TreeNode<SimpleNode>(new SimpleNode("a"));
        //    {
        //        var node0 = root.AddChild(new SimpleNode("a-b1"));
        //        {
        //            var node01 = node0.AddChild(new SimpleNode("a-b1-c1"));
        //            {
        //                var node010 = node0.AddChild(new SimpleNode("a-b1-c1-d1"));
        //                {
        //                    var node0100 = node0.AddChild(new SimpleNode("a-b1-c1-d1-e1"));
        //                    var node0101 = node0.AddChild(new SimpleNode("a-b1-c1-d1-e2"));
        //                    var node0102 = node0.AddChild(new SimpleNode("a-b1-c1-d1-e3"));
        //                }
        //            }
        //        }

        //        var node1 = root.AddChild(new SimpleNode("a-b2"));
        //        {
        //            var node10 = node1.AddChild(new SimpleNode("a-b2-c1"));
        //            var node11 = node1.AddChild(new SimpleNode("a-b2-c2"));
        //        }
        //        var node2 = root.AddChild(new SimpleNode("a-b3"));
        //        {
        //            var node20 = node2.AddChild(new SimpleNode("a-b3-c1"));
        //            var node21 = node2.AddChild(new SimpleNode("a-b3-c2"));
        //            var node22 = node2.AddChild(new SimpleNode("a-b3-c3"));
        //            var node23 = node2.AddChild(new SimpleNode("a-b3-c4"));
        //            {
        //                var node230 = node2.AddChild(new SimpleNode("a-b3-c4-d1"));
        //                var node231 = node2.AddChild(new SimpleNode("a-b3-c4-d2"));
        //                var node232 = node2.AddChild(new SimpleNode("a-b3-c4-d3"));
        //            }
        //        }
        //        var node3 = node2.AddChild(new SimpleNode("a-b4"));
        //    }
        //    return root;
        //}

        #endregion
    }
}