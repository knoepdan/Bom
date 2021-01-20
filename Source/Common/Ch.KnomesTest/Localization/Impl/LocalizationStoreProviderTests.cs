using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ch.Knomes.TestUtils.Resources;
using Ch.Knomes.Localization.TestUtils;

using Xunit;

namespace Ch.Knomes.Localization.Impl
{
    public class LocalizationStoreProviderTests
    {
        [Fact]
        public void LocalizationProvider_can_read_json()
        {
            var store = LocalizationTestUtils.GetLocacationStore();

            // assert correct values  .. could be greatyl improved
            Assert.NotNull(store);
        }
    }
}