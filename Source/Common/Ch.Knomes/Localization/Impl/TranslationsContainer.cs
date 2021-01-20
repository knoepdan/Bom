using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch.Knomes.Localization.Impl
{
    public interface ITranslationsContainer
    {
        ITextItem? GetTextItem();
        ITextItem? GetTextItemByLanguage(string langCode);
    }

    public class TranslationsContainer : ITranslationsContainer
    {
        public TranslationsContainer(IList<ITextItem> items)
        {
            this._translations.AddRange(items);
        }

        public TranslationsContainer(ITextItem item)
        {
            this._translations.Add(item);
        }

        public ITextResolver Resolver { get; } = new TextResolver();

        private List<ITextItem> _translations = new List<ITextItem>();
        internal IReadOnlyList<ITextItem> Translations => _translations;

        public void AddItem(ITextItem item)
        {
            this._translations.Add(item);
        }

        #region interface ITranslationsContainer

        public ITextItem? GetTextItemByLanguage(string langCode)
        {
            return this.Resolver.GetTextItemByLanguage(langCode, this._translations);
        }

        public ITextItem? GetTextItem()
        {
            return this.Resolver.GetTextItem(this._translations);
        }

        #endregion
    }
}