using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Knomes.Localize.Resolver;
using Microsoft.AspNetCore.Html;

namespace Knomes.Localize.DataAnnotations
{
    /// <summary>
    /// Some helper methods for attributes
    /// </summary>
    public static class AnnotationHelper
    {
        /// <summary>
        /// Defines what happens when Localization is not present. 
        /// </summary>
        /// <remarks>Mainly for testing and to be used for finding translations with no resources! (Not threadsafe)</remarks>
        public static NoLocalizationPolicy NoLocPolicyForAttributes = NoLocalizationPolicy.Fallback;

        internal static ITextService GetTextService(ValidationAttribute attr)
        {
            var service = GetTextService(attr.ErrorMessageResourceName ?? "");
            return service;
        }

        internal static ITextService GetTextService(string errorMessageResourceName)
        {
            if (!string.IsNullOrEmpty(errorMessageResourceName) && LocalizationGlobals.GetDefaultTextServiceFunc != null)
            {
                var textService = LocalizationGlobals.GetDefaultTextServiceFunc();
                return textService;
            }

            var dummyService = new DummyTextservice();
            switch (AnnotationHelper.NoLocPolicyForAttributes)
            {
                case NoLocalizationPolicy.Fallback:
                    return dummyService;
                case NoLocalizationPolicy.FlagMessasge:
                    return new MissingLocalizationFinderTextService(dummyService, NoLocPolicyForAttributes);
                case NoLocalizationPolicy.ThrowException:
                    System.Diagnostics.Debug.WriteLine($"No defined Textservice. ErrorMessageResourceName: '{errorMessageResourceName}'(GetTextservice function exists: { LocalizationGlobals.GetDefaultTextServiceFunc != null}) ");
                    return new MissingLocalizationFinderTextService(dummyService, NoLocPolicyForAttributes);
                default:
                    throw new Exception($"Unknown NoLocalizationPolicy in AnnotationHelper: " + AnnotationHelper.NoLocPolicyForAttributes);
            }
        }
    }
}
