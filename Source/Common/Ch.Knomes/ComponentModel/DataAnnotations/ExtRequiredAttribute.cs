using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Ch.Knomes.Localization;

namespace Ch.Knomes.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Like RequiredAttribute but uses <see cref="LocalizationGlobals.GetDefaultTextServiceFunc"/> for localization
    /// </summary>
    public class ExtRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            // ensure default value
            string errorMessage = "The {0} field is required."; //base.FormatErrorMessage(name);

            // translate using localizer
            if (!string.IsNullOrEmpty(this.ErrorMessageResourceName) && LocalizationGlobals.GetDefaultTextServiceFunc != null)
            {
                var textService = LocalizationGlobals.GetDefaultTextServiceFunc();
                if(textService != null)
                {
                    errorMessage = textService.Localize(this.ErrorMessageResourceName, errorMessage, name);
                }
            }
            return errorMessage;
        }
    }
}
