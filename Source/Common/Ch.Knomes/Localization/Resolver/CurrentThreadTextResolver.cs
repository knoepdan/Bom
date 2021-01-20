using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch.Knomes.Localization.Utils;

namespace Ch.Knomes.Localization.Resolver
{
    public class CurrentThreadTextResolver : TextResolverBase { 

        protected override string GetCurrentLangCode()
        {
            var transCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            if (transCulture != null)
            {
                return transCulture.Name.ToLowerInvariant(); // "en" or "en-us" ;
            }
            return "en";
        }
    }
}