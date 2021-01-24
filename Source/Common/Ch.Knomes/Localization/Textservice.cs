using Microsoft.AspNetCore.Html;
using System.Web;

using Ch.Knomes.Localization.Store;
using Ch.Knomes.Localization.Resolver;
using Ch.Knomes.Localization.Utils;

namespace Ch.Knomes.Localization
{
    /// <summary>
    /// Dummy implementention of Text-/Htmlservice not translating anything but more or less returning the fallback value
    /// </summary>
    /// <remarks>When HtmlString is returned, values are encoded</remarks>
    public class Textservice: ITextService, IHtmlService
    {
        public Textservice(ILocalizationStore store, ITextResolver? resolver = null)
        {
            this.Store = store;
            if(resolver == null)
            {
                resolver = new CurrentThreadTextResolver();
            }
            this.Resolver = resolver;
        }

        public ILocalizationStore Store { get; }

        public ITextResolver Resolver { get; }

        #region ITextService

        public string Localize(string code, string fallbackValue, params object[] args)
        {
            ITextItem? item = this.GetTextItem(code);
            string? textValue = item?.Text;
            if (textValue == null)
            {
                textValue = fallbackValue;
            }
            return LocalizationUtility.FormatStringFailsafe(textValue, args);
        }

        private ITextItem? GetTextItem(string code)
        {
            ITranslationsContainer? foundTrans = this.Store.GetTranslationsOfCode(code);
            if (foundTrans != null)
            {
                return foundTrans.GetTextItem(this.Resolver);
            }
            return null;
        }

        public string Fixed(string textValue, params object[] args)
        {
            return LocalizationUtility.FormatStringFailsafe(textValue, args);
        }

        public string Todo(string code, string fallbackValue, params object[] args)
        {
            return LocalizationUtility.FormatStringFailsafe(fallbackValue, args);
        }

        public string GetCurrentLangCode()
        {
            return this.Resolver.GetCurrentLangCode();
        }

        #endregion

        #region IHtmlService

        public HtmlString LocalizeHtml(string code, string htmlFallbackValue, params object[] args)
        {
            string? htmlText;
            var encodedParams = LocalizationUtility.EncodedParams(args);
            ITextItem? item = this.GetTextItem(code);
            if (item != null)
            {
                if (item.Type == TextType.Html)
                {
                    htmlText = LocalizationUtility.FormatStringFailsafe(item.Text, encodedParams);
                }
                else
                {
                    // is not html -> we need to encode the language
                    htmlText = LocalizationUtility.FormatStringFailsafe(HttpUtility.HtmlEncode(item.Text), encodedParams); HttpUtility.HtmlEncode(item.Text);
                }
            }
            else
            {
                // fallback -> fallback value is treated as html
                htmlText = LocalizationUtility.FormatStringFailsafe(htmlFallbackValue, encodedParams);
            }

            return new HtmlString(htmlText);
        }

        public HtmlString FixedHtml(string htmlText, params object[] args)
        {
            var encodedParams = LocalizationUtility.EncodedParams(args);
            var htmlTextWithParams = LocalizationUtility.FormatStringFailsafe(htmlText, encodedParams);
            return new HtmlString(htmlTextWithParams);

        }

        public HtmlString TodoHtml(string code, string htmlFallbackValue, params object[] args)
        {
            var encodedParams = LocalizationUtility.EncodedParams(args);
            var htmlText = LocalizationUtility.FormatStringFailsafe(htmlFallbackValue, encodedParams);
            return new HtmlString(htmlText);
        }

        #endregion
    }
    public class Textservice<TLocalizationStore, TResolver> : Textservice, ITextService<TResolver>, IHtmlService<TResolver> where TLocalizationStore : ILocalizationStore where TResolver : ITextResolver
    {
        public Textservice(TLocalizationStore store, TResolver resolver) : base(store, resolver)
        {

        }

        public TLocalizationStore LocalizationSTroe => (TLocalizationStore)base.Store;

        public TResolver TextResolver => (TResolver)base.Resolver;

        TResolver ITextService<TResolver>.Resolver => (TResolver)base.Resolver;

        TResolver IHtmlService<TResolver>.Resolver => (TResolver)base.Resolver;
    }
}
