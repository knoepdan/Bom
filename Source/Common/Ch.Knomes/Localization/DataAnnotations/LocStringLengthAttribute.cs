using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Ch.Knomes.Localization;

namespace Ch.Knomes.Localization.DataAnnotations
{
    public class LocStringLengthAttribute : StringLengthAttribute
    {

        public LocStringLengthAttribute(int maximumLength) : base(maximumLength)
        {

        }

        public string? LocalizationKeyPrefix { get; set; }

        public override string FormatErrorMessage(string name)
        {
            // ensure default value
            string errorMessage = "";
            string localizationKey = "";
            if (this.MinimumLength > 0)
            {
                if(this.MaximumLength == MinimumLength)
                {
                    errorMessage = "The {0} value must have " + this.MaximumLength + " characters.";
                    localizationKey = "Common.Validation.String.FixedLength";
                }
                else if (this.MaximumLength > this.MaximumLength)
                {
                    errorMessage = "The {0} value must have a minimum length of " + this.MinimumLength + " and may not exceed " + this.MaximumLength + " characters.";
                    localizationKey = "Common.Validation.String.MinMaxLength";
                }
                else
                {
                    errorMessage = "The {0} value must have a minimum length of " + this.MinimumLength + ".";
                    localizationKey = "Common.Validation.String.MinLength";
                }
            }
            else
            {
                errorMessage = "The {0} value may not exceed " + this.MaximumLength + " characters.";
                localizationKey = "Common.Validation.String.MaxLength";
            }

            // translate using localizer
            if (LocalizationGlobals.GetDefaultTextServiceFunc != null)
            {
                var textService = LocalizationGlobals.GetDefaultTextServiceFunc();
                if (textService != null)
                {
                    if (!string.IsNullOrEmpty(base.ErrorMessageResourceName))
                    {
                        errorMessage = textService.Localize(this.ErrorMessageResourceName, errorMessage, name);
                    }
                    else
                    {
                        errorMessage = textService.Localize(localizationKey, errorMessage, name);
                    }
                }
            }
            return errorMessage;
        }

    }
}
