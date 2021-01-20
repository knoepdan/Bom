using Microsoft.AspNetCore.Html;
using System.Web;

using Ch.Knomes.Localization.Impl;
using Ch.Knomes.Localization.Utils;

namespace Ch.Knomes.Localization
{
    /// <summary>
    /// Dummy implementention of Text-/Htmlservice not translating anything but more or less returning the fallback value
    /// </summary>
    /// <remarks>When HtmlString is returned, values are encoded</remarks>
    public class Textservice : ITextService, IHtmlService
    {
        public Textservice(ILocalizationStore store)
        {
            this._store = store;
        }

        private readonly ILocalizationStore _store;

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
            ITranslationsContainer? foundTrans = this._store.GetTranslationsOfCode(code);
            if (foundTrans != null)
            {
                return foundTrans.GetTextItem();
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

        #endregion

        #region IHtmlService

        public HtmlString LocalizeHtml(string code, string htmlFallbackValue, params object[] args)
        {
            string? htmlText;
            var encodedParams = LocalizationUtility.EncodedParams(args);
            ITextItem? item = this.GetTextItem(code);
            if(item != null)
            {
                if (item.Type == TextType.Html)
                {
                    htmlText = LocalizationUtility.FormatStringFailsafe(item.Text, encodedParams);
                 }
                else
                {
                    // is not html -> we need to encode the language
                    htmlText = LocalizationUtility.FormatStringFailsafe(HttpUtility.HtmlEncode(item.Text), encodedParams);  HttpUtility.HtmlEncode(item.Text);
                }
            }
            else
            {
                // fallback -> fallback value is treated as html
                htmlText = LocalizationUtility.FormatStringFailsafe(htmlFallbackValue, encodedParams);
            }

            return new HtmlString(htmlText);
        }

        HtmlString IHtmlService.FixedHtml(string htmlText, params object[] args)
        {
            var encodedParams = LocalizationUtility.EncodedParams(args);
            var htmlTextWithParams = LocalizationUtility.FormatStringFailsafe(htmlText, encodedParams);
            return new HtmlString(htmlTextWithParams);

        }

        HtmlString IHtmlService.TodoHtml(string code, string htmlFallbackValue, params object[] args)
        {
            var encodedParams = LocalizationUtility.EncodedParams(args);
            var htmlText = LocalizationUtility.FormatStringFailsafe(htmlFallbackValue, encodedParams);
            return new HtmlString(htmlText);
        }

        #endregion
    }
}
