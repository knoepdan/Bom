﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Knomes.Localize;

namespace Knomes.Localize.DataAnnotations
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
            ITextService textService = AnnotationHelper.GetTextService(this);
            errorMessage = textService.Localize(this.ErrorMessageResourceName ?? "", errorMessage, name);
            return errorMessage;
        }
    }
}
