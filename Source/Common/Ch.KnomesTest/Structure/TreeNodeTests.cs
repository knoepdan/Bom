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
            var visual = rootNode.VisualStringRepresentation;
            Assert.True(!string.IsNullOrWhiteSpace(visual));
        }
    }
}
