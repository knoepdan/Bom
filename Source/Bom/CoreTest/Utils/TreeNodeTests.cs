using System;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Model;
using Bom.Utils.Math;
using Bom.Core.Testing;

namespace Bom.Core.Utils
{
    public class TreeNodeTests
    {
        [Fact]
        public void Can_get_visual_representation_of_InMemoryTree()
        {
            var rootNode = TestDataFactory.CreateSampleNodes(4, 4);
            var visual = rootNode.VisualStringRepresentation;
            Assert.True(!string.IsNullOrWhiteSpace(visual));
        }
    }
}
