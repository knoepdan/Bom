using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Knomes.Localize;

namespace Knomes.Localize.DataAnnotations
{
    /// <summary>
    /// Like RequiredAttribute but uses <see cref="LocalizationGlobals.GetDefaultTextServiceFunc"/> for localization
    /// </summary>
    public class LocRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            string errorMessage = "The '{0}' field is required."; // fallback value

            // translate using localizer
            ITextService textService = AnnotationHelper.GetTextService(this);
            errorMessage = textService.Localize(this.ErrorMessageResourceName ?? "", errorMessage, name);
            return errorMessage;
        }

    }
}
