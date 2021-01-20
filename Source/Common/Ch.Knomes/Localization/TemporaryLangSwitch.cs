using System;
using System.Globalization;

namespace Ch.Knomes.Localization
{

    /// <summary>
    /// To be used to set the CurrentUICulture temporarily to another culture. -> To be used with the using statement
    /// </summary>
    public class TemporaryLangSwitch : IDisposable
    {
        public CultureInfo OriginalCulture { get; }


        /// <summary>
        /// Switches UICulture (responsible for validation) until dispose is called
        /// </summary>
        /// <param name="langCode">Lang-Code: "en" or "en-Us". If culture is not found, UICulture is not switched</param>
        public TemporaryLangSwitch(string langCode)
        {
            this.OriginalCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var tempCulture = CultureInfo.GetCultureInfo(langCode);
            if (tempCulture != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = tempCulture;
            }
        }

        public TemporaryLangSwitch(CultureInfo temporaryCulture)
        {
            this.OriginalCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = temporaryCulture;
        }


        void IDisposable.Dispose()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = OriginalCulture;
        }
    }
}
