using System;
using System.Linq;
using Xunit;

using Ch.Knomes.Structure.Testing;

namespace Ch.Knomes.Structure
{
    public class TreeNodeTests
    {
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
        public void Move_node_down_with_children_will_throw()
        {
            var rootNode = TestDataFactory.CreateSampleAnimalNodes();
            var node = rootNode.DescendantsAndI.Where(n => n.Level == 2 && n.Children.Any()).First();
            var targetParent = rootNode.DescendantsAndI.Where(n => n.Level == 4 && n.Ancestors.Any(a => a == node)).First();
            try
            {
                node.MoveToNewParent(targetParent, true);
                Assert.True(1 == 2); // trigger fail
            }
            catch (Exception ex)
            {
                var formerChildren = node.Children.ToList();
                var formerParent = node.Parent;
                node.MoveToNewParent(targetParent, false); //this works because we only pick one node
                Assert.True(node.Children.Count == 0); // no children as they have not been moved (but were moved to parent)
                foreach(var formerChild in formerChildren)
                {
                    Assert.Contains(formerParent.Children, c => c == formerChild);
                }
            }
        }
    }
}
