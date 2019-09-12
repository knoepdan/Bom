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
    }
}
