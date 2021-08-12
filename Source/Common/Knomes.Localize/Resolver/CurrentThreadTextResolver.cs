using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knomes.Localize.Utils;

namespace Knomes.Localize.Resolver
{
    public class CurrentThreadTextResolver : TextResolverBase { 

        public override string GetCurrentLangCode()
        {
            var transCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            if (transCulture != null)
            {
                return LocalizationUtility.TrimmLangCodeForComparisons(transCulture.Name);  // "en" or "en-us" ;
            }
            return "en";
        }

        #region language switch

        public override ITemporaryLanguageSwitch? GetTemporayLanguageSwitch(string langCode)
        {
            if (string.IsNullOrEmpty(langCode))
            {
                throw new ArgumentException("Passed empty langCode", nameof(langCode));
            }
            var langSwitch = new CurrentThreadTextResolverSwitch(langCode);
            return langSwitch;
        }

        /// <summary>
        /// Temparariyl changes the  CurrentUICulture which is used for localization by the CurrentThreadTextResolver. Attention: always use in a using statement
        /// </summary>
        /// <remarks>
        /// Usage: 
        /// 
        /// using(var switch = resolver.GetTemporaryLanguageSwitch("en)){
        ///     TextService.Localize("someCode", ".....);
        /// }
        ///  or
        /// using(var switch = new CurrentThreadTextResolver.CurrentThreadTextResolverSwitch("en"){
        ///     TextService.Localize("someCode", ".....);
        /// }
        /// </remarks>
        public class CurrentThreadTextResolverSwitch : ITemporaryLanguageSwitch
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

        #endregion
    }
}