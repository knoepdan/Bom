using System;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Nodes.DbModels;
using Bom.Utils.Math;

namespace Bom.Core.Nodes
{
    public class PathTests
    {
        [Fact]
        public void PathSeparator_is_not_part_of_numerical_system_digits()
        {
            Assert.False(NumericalSystemHelper.Digits.Contains(Path.Separator)); // Pathseparator
        }
    }
}
