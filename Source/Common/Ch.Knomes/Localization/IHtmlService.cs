using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Html;
using Ch.Knomes.Localization.Resolver;

namespace Ch.Knomes.Localization
{

    public interface IBasicHtmlService
    {
        /// <summary>
        /// Translate value in the corresponding language
        /// </summary>
        /// <param name="code">key</param>
        /// <param name="fallbackValue">Fallback if no value was found</param>
        /// <param name="args">param objects</param>
        /// <returns></returns>
        HtmlString LocalizeHtml(string code, string fallbackValue, params object[] args);
    }

    /// <summary>
    /// Service to localize text, already taking care of html encoding
    /// </summary>
    public interface IHtmlService : IBasicHtmlService
    {
        /// <summary>
        /// Marker for text that don't need translation
        /// </summary>
        /// <param name="textValue"></param>
        HtmlString FixedHtml(string textValue, params object[] args);

        /// <summary>
        /// Text that don't need translation
        /// </summary>
        /// <param name="textValue"></param>
        HtmlString TodoHtml(string code, string fallbackValue, params object[] args);

        /// <summary>
        /// Resolver choosing the language
        /// </summary>
        ITextResolver Resolver { get; }
    }

    public interface IHtmlService<TResolver> where TResolver : ITextResolver
    {

        /// <summary>
        /// Resolver choosing the language
        /// </summary>
        TResolver Resolver { get; }
    }
}
