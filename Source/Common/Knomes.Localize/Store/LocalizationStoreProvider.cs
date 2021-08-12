using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

using Knomes.Localize.Utils;

namespace Knomes.Localize.Store
{
    public class LocalizationStoreProvider
    {
        public static LocalizationStore ReadInLocalizationStore(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentException("Passed json may not be null or empty", nameof(json));
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = {  
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
            IList<CodeTextItem>? deserializedList = JsonSerializer.Deserialize<List<CodeTextItem>>(json, options);
            if (deserializedList == null)
            {
                throw new ArgumentException("Passed json could not be deserialized: " + json, nameof(json));
            }
            IEnumerable<ITextItemWithCode> texts = EnsureLangCodeIsLowercase(deserializedList);
            var store = new LocalizationStore(texts);
            return store;
        }

        private static IEnumerable<ITextItemWithCode> EnsureLangCodeIsLowercase(IList<CodeTextItem> list)
        {
            foreach(var item in list)
            {
                EnsureLangCodeIsLowercase(item);
                yield return item;
            }
        }

        private static void EnsureLangCodeIsLowercase(CodeTextItem item)
        {
            item.LangCode = LocalizationUtility.TrimmLangCodeForComparisons(item.LangCode);
        }
    }
}
