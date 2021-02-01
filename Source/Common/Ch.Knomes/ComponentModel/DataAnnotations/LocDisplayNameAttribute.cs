using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ch.Knomes.Localization;

namespace Ch.Knomes.ComponentModel.DataAnnotations
{
    public class LocDisplayNameAttribute : DisplayNameAttribute
    {
        public LocDisplayNameAttribute(string fallbackName, string localizationKey = "") : base(fallbackName)
        {
            this.LocalizationKey = localizationKey;
        }

        public string LocalizationKey { get; }

        public override string DisplayName
        {
            get
            {
                var name = base.DisplayName;
                if (LocalizationGlobals.GetDefaultTextServiceFunc != null)
                {
                    var textService = LocalizationGlobals.GetDefaultTextServiceFunc();
                    if (textService != null)
                    {
                        if (!string.IsNullOrEmpty(this.LocalizationKey))
                        {
                            name = textService.Localize(this.LocalizationKey, name);
                        }
                        else
                        {
                            name = textService.Localize($"Common.Display.Model.{name}", name); // by convention
                        }
                    }
                }

                return name;
            }
        }
    }
}
