using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch.Knomes.Localization.Impl
{
    public interface ILocalizationStore
    {
        ITranslationsContainer? GetTranslationsOfCode(string code);
    }

    public class LocalizationStore : ILocalizationStore
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

        #endregion
    }
}