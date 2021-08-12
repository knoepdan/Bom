using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knomes.Localize.Utils;

namespace Knomes.Localize.Store
{
    public interface ILocalizationStore
    {
        ITranslationsContainer? GetTranslationsOfCode(string code);
    }

    public interface IInfoLocalizationStore : ILocalizationStore
    {
        IReadOnlyList<LanguageCodeInfo> GetAvailableLanguageStatistics();

        IReadOnlyList<string> GetAvailableLanguageCodes();

        bool HasTranslationsForLangCode(string? langCode, bool exactMatchIncludingCulture = false);
    }

    public class LocalizationStore : IInfoLocalizationStore
    {
        public LocalizationStore(IEnumerable<ITextItemWithCode> allItems)
        {
            if (allItems == null)
            {
                throw new ArgumentNullException(nameof(allItems));
            }
            this._codeTranslationDic = GroupByCode(allItems);
        }

        public LocalizationStore(IDictionary<string, IList<ITextItem>> translatedTextsDic)
        {
            if (translatedTextsDic == null)
            {
                throw new ArgumentNullException(nameof(translatedTextsDic));
            }
            var dic = new Dictionary<string, ITranslationsContainer>();
            foreach (var key in translatedTextsDic.Keys)
            {
                var translations = translatedTextsDic[key];
                var container = new TranslationsContainer(translations);
                dic.Add(key, container);
            }
            this._codeTranslationDic = dic;
        }

        public LocalizationStore(Dictionary<string, ITranslationsContainer> translatedTextsDic)
        {
            if (translatedTextsDic == null)
            {
                throw new ArgumentNullException(nameof(translatedTextsDic));
            }
            this._codeTranslationDic = translatedTextsDic;
        }

        private readonly Dictionary<string, ITranslationsContainer> _codeTranslationDic = new Dictionary<string, ITranslationsContainer>();

        public IReadOnlyDictionary<string, ITranslationsContainer> CodeTranslationDic => this._codeTranslationDic;

        private Dictionary<string, ITranslationsContainer> GroupByCode(IEnumerable<ITextItemWithCode> allItems)
        {
            if (allItems == null)
            {
                throw new ArgumentNullException(nameof(allItems));
            }

            var dic = new Dictionary<string, ITranslationsContainer>();
            foreach (var item in allItems)
            {
                ITranslationsContainer? transContainer;
                if (!dic.TryGetValue(item.Code, out transContainer))
                {
                    var container = new TranslationsContainer(item);
                    dic.Add(item.Code, container);
                }
                else
                {
                    var transContainerImpl = (TranslationsContainer)transContainer;
                    transContainerImpl.AddItem(item);
                }
            }
            return dic;
        }

        #region interface

        public ITranslationsContainer? GetTranslationsOfCode(string code)
        {
            ITranslationsContainer? itemsforCode;
            if (_codeTranslationDic.TryGetValue(code, out itemsforCode))
            {
                return itemsforCode;
            }
            return null;
        }


        private List<LanguageCodeInfo>? _languageCodeInfos = null;
        public IReadOnlyList<LanguageCodeInfo> GetAvailableLanguageStatistics()
        {
            if (this._languageCodeInfos == null)
            {
                this._languageCodeInfos = CalculateAvailableLanguages().ToList();
            }
            return this._languageCodeInfos;
        }

        public IReadOnlyList<string> GetAvailableLanguageCodes()
        {
            var stats = GetAvailableLanguageStatistics();
            var langCodes = stats.Select(x => x.LanguageCode);
            return langCodes.ToList();
        }

        public bool HasTranslationsForLangCode(string? langCode, bool exactMatchIncludingCulture = false)
        {
            if (string.IsNullOrEmpty(langCode))
            {
                return false;
            }
            var trimmedCode = LocalizationUtility.TrimmLangCodeForComparisons(langCode);
            var availableLangCodes = GetAvailableLanguageCodes();
            bool isExactMatch = availableLangCodes.Any(x => x == trimmedCode);
            if (isExactMatch)
            {
                return true;
            }
            else if (exactMatchIncludingCulture)
            {
                return false;
            }
            var parentLangCode = LocalizationUtility.GetParentLanguageCode(trimmedCode);
            if (parentLangCode != null)
            {
                var val = GetAvailableLanguageCodes().Any(x => x == parentLangCode);
                return val;
            }
            return false;
        }

        #endregion

        #region other methods

        public void ResetLanguageStatisticsCache()
        {
            this._languageCodeInfos = null;
        }

        public IEnumerable<LanguageCodeInfo> CalculateAvailableLanguages()
        {
            var allLanguageCodes = new List<string>();
            foreach (var trans in this._codeTranslationDic.Values)
            {
                var langCodes = trans.GetLanguageCodes();
                allLanguageCodes.AddRange(langCodes);
            }

            // group and count
            var grouped = allLanguageCodes.GroupBy(s => s)
                .Select(g => new { Symbol = g.Key, Count = g.Count() });

            foreach (var g in grouped)
            {
                yield return new LanguageCodeInfo(g.Symbol, g.Count);
            }
        }

        #endregion
    }
}