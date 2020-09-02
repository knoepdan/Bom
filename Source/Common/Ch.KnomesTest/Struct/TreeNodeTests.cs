using System;
using System.Linq;
using Xunit;

using Ch.Knomes.Struct.Testing;

namespace Ch.Knomes.Struct
{
    public class TreeNodeTests
    {
        [Fact]
        public void Creating_basic_tree_works()
        {
            var root = new TreeNode<SimpleNode>(new SimpleNode("Animals"));
            Assert.True(root.Parent == null && root.Children.Count() == 0 && root.Descendants.Count() == 0 && root.DescendantsAndI.Count() == 1 && root.Ancestors.Count() == 0 && root.Siblings.Count == 0);
            var n1 = root.AddChild(new SimpleNode("Mammals"));
            Assert.True(root.Parent == null && root.Children.Count() == 1 && root.Descendants.Count() == 1 && root.DescendantsAndI.Count() == 2 && root.Ancestors.Count() == 0 && root.Siblings.Count == 0);
            Assert.True(n1.Parent == root && n1.Children.Count() == 0 && n1.Descendants.Count() == 0 && n1.DescendantsAndI.Count() == 1 && n1.Ancestors.Count() == 1 && n1.Siblings.Count == 0);
            var n2 = root.AddChild(new SimpleNode("Reptiles"));
            Assert.True(root.Parent == null && root.Children.Count() == 2 && root.Descendants.Count() == 2 && root.DescendantsAndI.Count() == 3 && root.Ancestors.Count() == 0 && root.Siblings.Count == 0);
            Assert.True(n1.Parent == root && n1.Children.Count() == 0 && n1.Descendants.Count() == 0 && n1.DescendantsAndI.Count() == 1 && n1.Ancestors.Count() == 1 && n1.Siblings.Count == 1);
            var nn3 = n1.AddChild(new SimpleNode("Elephant"));
            Assert.True(root.Parent == null && root.Children.Count() == 2 && root.Descendants.Count() == 3 && root.DescendantsAndI.Count() == 4 && root.Ancestors.Count() == 0 && root.Siblings.Count == 0);
            Assert.True(n1.Parent == root && n1.Children.Count() == 1 && n1.Descendants.Count() == 1 && n1.DescendantsAndI.Count() == 2 && n1.Ancestors.Count() == 1 && n1.Ancestors.Contains(root) && n1.Siblings.Count == 1);
            Assert.True(nn3.Parent == n1 && nn3.Children.Count() == 0 && nn3.Descendants.Count() == 0 && nn3.DescendantsAndI.Count() == 1 &&
                nn3.Ancestors.Count() == 2 && nn3.Ancestors.Contains(n1) && nn3.Ancestors.Contains(root) && nn3.Siblings.Count == 0);
        }

        [Fact]
        public void Can_get_visual_representation_of_InMemoryTree()
        {
            var rootNode = TestDataFactory.CreateSampleAnimalNodes();
            var visual = rootNode.VisualStringRepresentation();
            Assert.True(!string.IsNullOrWhiteSpace(visual));
        }

        [Fact]
        public void Compare_of_two_InMemoryTree_works()
        {
            var rootNode = TestDataFactory.CreateSampleAnimalNodes();
            var rootNode2 = TestDataFactory.CreateSampleAnimalNodes();
            var areEual = rootNode.AreDescendantsAndIEqual(rootNode2);
            Assert.True(areEual);
            // change on and it has to fail
            rootNode2.Descendants.Last().Data.Title = "asdfsdf";
            var areEual2 = rootNode.AreDescendantsAndIEqual(rootNode2);
            Assert.False(areEual2);
        }

        [Fact]
        public void Remove_node_from_tree_works()
        {
            var rootNode = TestDataFactory.CreateSampleAnimalNodes();
            var countBefore = rootNode.DescendantsAndI.Count();
            var node = rootNode.DescendantsAndI.Where(n => n.Level == 2 && n.Children.Any()).First();
            var child = node.Children.First();

            node.RemoveChild(child);

            Assert.True(node.Children.All(x => x != child));
            Assert.True(child.Parent == null);
            Assert.True(countBefore == rootNode.DescendantsAndI.Count() + child.DescendantsAndI.Count());
        }

        [Fact]
        public void Extract_node_from_tree_works()
        {
            var rootNode = TestDataFactory.CreateSampleAnimalNodes();
            var countBefore = rootNode.DescendantsAndI.Count();
            var node = rootNode.DescendantsAndI.Where(n => n.Level == 2 && n.Children.Any()).First();

            node.ExtractNodeFromTree();
            Assert.True(rootNode.Children.All(x => x != node));
            Assert.True(node.Parent == null && node.Children.Count == 0);
            Assert.True(countBefore == rootNode.DescendantsAndI.Count() + 1);
        }

        [Fact]
        public void Move_node_down_with_children_will_throw()
        {
            var rootNode = TestDataFactory.CreateSampleAnimalNodes();
            var node = rootNode.DescendantsAndI.Where(n => n.Level == 2 && n.Children.Any()).First();
            var targetParent = rootNode.DescendantsAndI.Where(n => n.Level == 4 && n.Ancestors.Any(a => a == node)).First();
            try
            {
                node.MoveToNewParent(targetParent, true);
            }
            catch (Exception)
            {
                var formerChildren = node.Children.ToList();
                var formerParent = node.Parent;
                node.MoveToNewParent(targetParent, false); //this works because we only pick one node
                Assert.True(node.Children.Count == 0); // no children as they have not been moved (but were moved to parent)
                foreach (var formerChild in formerChildren)
                {
                    if (formerParent == null)
                    {
                        throw new Exception($"Former parent may not be null! ({formerChild})");
                    }
                    Assert.Contains(formerParent.Children, c => c == formerChild);
                }
                return;
            }

            // if code reaches here, we have an error
            Assert.True(1 == 2); // trigger fail
        }

        [Fact]
        public void Ancestors_are_sorted_by_proximity_to_node()
        {
            var rootNode = TestDataFactory.CreateSampleAnimalNodes();
            var leaveNode = rootNode.Descendants.First(x => x.Children.Count == 0);
            var ancestors = leaveNode.Ancestors.ToList();
            Assert.True(ancestors.Count > 3 && ancestors[0] == leaveNode.Parent && ancestors[ancestors.Count - 1] == rootNode); // basic tests and making sure we have ancestors

            var parent = leaveNode.Parent;
            int ancestorsIndex = 0;
            while(parent != null)
            {
                var ancestor = ancestors[ancestorsIndex];
                Assert.True(parent == ancestor);

                // prepare next loop
                parent = parent.Parent;
                ancestorsIndex++;
            }
        }
    }
}

