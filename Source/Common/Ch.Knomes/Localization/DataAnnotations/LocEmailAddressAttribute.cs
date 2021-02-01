using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ch.Knomes.Localization;

namespace Ch.Knomes.Localization.DataAnnotations
{
    public class LocEmailAddressAttribute : DataTypeAttribute
    {

        public LocEmailAddressAttribute() : base(DataType.EmailAddress)
        {

        }

        public override string FormatErrorMessage(string name)
        {
            string errorMessage = "The email address is not valid.";

            // translate using localizer
            if (!string.IsNullOrEmpty(this.ErrorMessageResourceName) && LocalizationGlobals.GetDefaultTextServiceFunc != null)
            {
                var textService = LocalizationGlobals.GetDefaultTextServiceFunc();
                if (textService != null)
                {
                    errorMessage = textService.Localize(this.ErrorMessageResourceName, errorMessage, name);
                }
            }
            return errorMessage;
        }
    }
}
