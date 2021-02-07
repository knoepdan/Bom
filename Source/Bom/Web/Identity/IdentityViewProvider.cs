using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bom.Web.Identity
{

    /// <summary>
    /// Returns path to views 
    /// </summary>
    /// 
    public class IdentityViewProvider
    {
        public const string AccountLogin= "~/Identity/Views/Account/Login.cshtml";


        public const string AccountRegister = "~/Identity/Views/Account/Register.cshtml"; 

        public const string FacebookRegister = "~/Identity/Views/Facebook/Register.cshtml";


        public const string SharedOAuthRegistration = "~/Identity/Views/Shared/OAuthRegistration.cshtml"; //"~/Areas/Identity/Views/Shared/OAuthRegistration.cshtml";

        public const string SharedErorr = "~/Identity/Views/Shared/Error.cshtml";

        public const string DefaultView = "~/Identity/Views/Home/Index.cshtml"; //"~/Areas/Identity/Views/Shared/OAuthRegistration.cshtml";
    }
}
