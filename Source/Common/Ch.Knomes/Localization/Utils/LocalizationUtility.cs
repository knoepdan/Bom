using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace Ch.Knomes.Localization.Utils
{
    public static class LocalizationUtility
    {
        #region public methods

        /// <summary>
        /// Trims passed languageCode to language part. Example: "en-Us" -> "en"
        /// </summary>
        /// <remarks>does no validation if it is a language string. Just a best guess to get the languagePart</remarks>
        public static string TrimToLanguagePart(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
            {
                return languageCode;
            }
            languageCode = languageCode.TrimStart();
            if(languageCode.Length  > 2)
            {
                return languageCode.Substring(0, 2);
            }
            return languageCode;
        }

        #endregion

        #region internal but quite generic

        /// <summary>
        /// Basic check whether passed language code is valid (eg: "en" or "en-US" or "en-us")
        /// </summary>
        /// <remarks>not a 100% check </remarks>
        internal static bool IsProbablyValidLanguageCode(string langCode)
        {
            if (string.IsNullOrWhiteSpace(langCode))
            {
                return false;
            }
            langCode = langCode.Trim();
            if (langCode.Length == 2 || langCode.Length > 3 && langCode[2] == '-')
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Transforms the language code into a more general form (eg: "de-ch" -> "de")
        /// </summary>
        ///<remarks>Currently only supports step from 2 level to one level (eg: "en-us" -> "en")</remarks>
        internal static string? GetParentLanguageCode(string langCode)
        {
            if (langCode != null && langCode.Length > 2 && langCode[2] == '-')
            {
                // passed langauge had local culture -> fallback to language only  (eg: "en-us" -> "en")
                return langCode.Substring(0, 2);
            }
            return null;
        }

        #endregion

        #region intern 

        /// <summary>
        /// Formats string similar to string.Format but is failsafe (in case string does not provide enough params: it will 
        /// </summary>
        internal static string FormatStringFailsafe(string text, IEnumerable<object> args)
        {
            try
            {
                return FormatString(text, args);
            }
            catch (FormatException)
            {
                // Example: 2 params passed but string only has one placeholder
                System.Diagnostics.Debug.WriteLine($"Swallowed FormatException in '{nameof(FormatStringFailsafe)}'. Passed text: '{text}'");
                return text;
            }
        }
        internal static IEnumerable<string> EncodedParams(IEnumerable<object> args)
        {
            if (args == null)
            {
                yield return "";
            }
            else
            {
#pragma warning disable CS8603 // Possible null reference return.
                foreach (object a in args)
                {
                    if (a == null)
                    {
                        yield return "";
                    }
                    else if (a is HtmlString)
                    {
                        yield return a.ToString(); // no need to encode as this is intended
                    }
                    else
                    {
                        yield return HttpUtility.HtmlEncode(a.ToString());
                    }
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
        }

       

        /// <summary>
        /// Ensures langCode can be compared easily via == (trimmes, toLowerInvariatne etc)
        /// </summary>
        /// <param name="langCode">langCode</param>
        /// <returns>langCode, trimmed and lowercase for simpler comparisons</returns>
        internal static string TrimmLangCodeForComparisons(string langCode)
        {
            if (string.IsNullOrEmpty(langCode))
            {
                return langCode;
            }
            return langCode.Trim().ToLowerInvariant();
        }

        #endregion

        #region private methods
        private static string FormatString(string text, IEnumerable<object> args)
        {
            if (string.IsNullOrEmpty(text) || args == null)
            {
                return text;
            }
            return string.Format(text, args.ToArray()); // ToArray is important otherwise it will be treated as one object
        }

        #endregion
    }
}