using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace Ch.Knomes.Localization.Utils
{
    internal static class LocalizationUtility
    {
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

        private static string FormatString(string text, IEnumerable<object> args)
        {
            if (string.IsNullOrEmpty(text) || args == null)
            {
                return text;
            }
            return string.Format(text, args.ToArray()); // ToArray is important otherwise it will be treated as one object
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
    }
}