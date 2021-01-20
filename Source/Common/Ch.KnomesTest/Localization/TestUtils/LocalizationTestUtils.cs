using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch.Knomes.TestUtils.Resources;
using Ch.Knomes.Localization;
using Ch.Knomes.Localization.Impl;

namespace Ch.Knomes.Localization.TestUtils
{
    public static class LocalizationTestUtils
    {
        public static string GetValidLocalizationJson()
        {
            var content = ResourceUtility.ReadManifestData<Impl.LocalizationStoreProviderTests>("localization.json");
            return content;
        }

        public static LocalizationStore? GetLocacationStore()
        {
            var json = LocalizationTestUtils.GetValidLocalizationJson();
            var store = LocalizationStoreProvider.ReadInLocalizationStore(json);
            return store;
   
        }
    }
}
