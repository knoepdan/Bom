using System;
using System.Linq;
using Xunit;
using Core.TestUtils;
using Core.Model;
using Utils.Math;

namespace Core
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
