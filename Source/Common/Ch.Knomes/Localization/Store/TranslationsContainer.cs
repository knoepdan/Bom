using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch.Knomes.Localization.Resolver;

namespace Ch.Knomes.Localization.Store
{
    public interface ITranslationsContainer
    {
        ITextItem? GetTextItem(ITextResolver resolver);
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

        private List<ITextItem> _translations = new List<ITextItem>();
        internal IReadOnlyList<ITextItem> Translations => _translations;

        public void AddItem(ITextItem item)
        {
            this._translations.Add(item);
        }

        #region interface ITranslationsContainer

        public ITextItem? GetTextItem(ITextResolver resolver)
        {
            return resolver.GetTextItem(this._translations);
        }

        #endregion
    }
}