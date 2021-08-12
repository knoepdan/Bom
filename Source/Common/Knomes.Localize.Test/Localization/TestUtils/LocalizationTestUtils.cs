using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch.Knomes.TestUtils.Resources;
using Knomes.Localize;
using Knomes.Localize.Store;

namespace Knomes.Localize.TestUtils
{
    public static class LocalizationTestUtils
    {
        public static string GetValidLocalizationJson()
        {
            var content = ResourceUtility.ReadManifestData<Store.LocalizationStoreProviderTests>("localization.json");
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
