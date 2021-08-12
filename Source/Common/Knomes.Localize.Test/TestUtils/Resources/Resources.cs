using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Ch.Knomes.TestUtils.Resources
{
    public static class Resources
    {
        private static string BaseLocation => Path.Combine(Path.Combine(TestHelper.ExecutionLocation, "TestUtils"), "Resources");

        public static string SmallPic => Path.Combine(BaseLocation, "smallPic.jpg");

    }
}
