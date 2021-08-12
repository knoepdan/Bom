using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Knomes.Localize;

namespace Knomes.Localize.DataAnnotations
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
                var localizationKey = GetLocalizationKey(name);
                var textService = AnnotationHelper.GetTextService(localizationKey);
                var localizedName = textService.Localize(this.LocalizationKey, name);
                return localizedName;
            }
        }

        private string GetLocalizationKey(string baseDisplayName)
        {
            if (!string.IsNullOrEmpty(this.LocalizationKey))
            {
                return this.LocalizationKey;
            }
            else
            {
                return ($"Common.Display.Model.{baseDisplayName}"); // by convention
            }
        }
    }
}
