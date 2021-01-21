using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ch.Knomes.TestUtils.Resources;
using Ch.Knomes.Localization.TestUtils;

using Xunit;

namespace Ch.Knomes.Localization.Store
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

        [Fact]
        public void LocalizationStore_returns_correct_statistics()
        {
            var store = LocalizationTestUtils.GetLocacationStore()!;
            Assert.NotNull(store);

            // assert correct values  .. could be greatyl improved
            var statistics = store.GetAvailableLanguageStatistics();
            var langCodes = store.GetAvailableLanguageCodes();

            Assert.True(statistics.Count == 4 && statistics.Any(x => x.LanguageCode == "en" && x.Count == 2));
            Assert.True(langCodes.Count == 4 && langCodes.Contains("en") && langCodes.Contains("en-us"));

            // HasTranslation tests
            Assert.True(store.HasTranslationsForLangCode("en", true));
            Assert.True(store.HasTranslationsForLangCode("en-us", true));
            Assert.True(store.HasTranslationsForLangCode("EN", true));
            Assert.False(store.HasTranslationsForLangCode("en-gb", true)); // false because of exact match
            Assert.True(store.HasTranslationsForLangCode("en-gb", false)); // en-gb is not here

            Assert.False(store.HasTranslationsForLangCode("sd", true));
            Assert.False(store.HasTranslationsForLangCode(null, true));
            Assert.False(store.HasTranslationsForLangCode("enn", false));
        }
    }
}