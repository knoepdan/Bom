using System;
using System.Globalization;

namespace Ch.Knomes.Localization.Resolver
{

    /// <summary>
    /// Temparariyl changes the  CurrentUICulture which is used for localization by the CurrentThreadTextResolver. Attention: always use in a using statement
    /// </summary>
    /// <remarks>
    /// Usage: 
    /// using(var switch = new CurrentThreadTextResolverSwitch("en"){
    ///     TextService.Localize("someCode", ".....);
    /// }
    /// </remarks>
    public class CurrentThreadTextResolverSwitch : IDisposable
    {
        public CultureInfo OriginalCulture { get; }


        /// <summary>
        /// Switches UICulture (responsible for localization) until dispose is called
        /// </summary>
        /// <param name="langCode">Lang-Code: "en" or "en-Us". If culture is not found, UICulture is not switched</param>
        public CurrentThreadTextResolverSwitch(string langCode)
        {
            this.OriginalCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var tempCulture = CultureInfo.GetCultureInfo(langCode);
            if (tempCulture != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = tempCulture;
            }
        }

        public CurrentThreadTextResolverSwitch(CultureInfo temporaryCulture)
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
