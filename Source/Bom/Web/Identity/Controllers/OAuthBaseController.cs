using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ch.Knomes.Security;
using Bom.Core.Common;
using Bom.Web.Common.Infrastructure;
using Bom.Web.Identity.Models;
using Bom.Core.Identity;
using Bom.Core.Identity.DbModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization;
using Bom.Web.Identity;

namespace Bom.Web.Identity.Controllers
{
    //  [Area("Identity")]
    //  [Route("facebook")]
    [Controller]
    public abstract class OAuthBaseController : BomBaseViewController
    {
        protected ModelContext Context { get; }

        public abstract string ProviderName {get;}


        public OAuthBaseController(ModelContext context)
        {
            Context = context;
        }


        protected User CreateNewOAuthUser(OAuthInfo oAuthInfo)
        {
            if(oAuthInfo == null)
            {
                throw new ArgumentNullException(nameof(oAuthInfo));
            }
           
            var user = new User();
            user.Username = oAuthInfo.Email ?? Guid.NewGuid().ToString();  // may not be null

            user.Salt = null;
            user.PasswordHash = null;
            user.ActivationToken = null;
            user.UserStatus = UserStatus.Initializing;

            user.Name = oAuthInfo.Name ?? "";
            user.Email2 = oAuthInfo.Email;


            //0 this._context.Users.Add(user);
            //  await this._context.SaveChangesAsync();


            return user;

        }

        [Authorize]
        [HttpGet("alreadyregistered")]
        public IActionResult AlreadyRegistered()
        {
            const string viewName = Bom.Web.Identity.IdentityViewProvider.SharedOAuthRegistration; ; //  "~/Areas/Identity/Views/Shared/OAuthRegistration.cshtml";
            var msg = new Mvc.UserMessage(this.TextService.Localize("Identity.Register.AlreadyRegisteredSuccess", "Congratulations, you are already registered."), Mvc.UserMessage.MessageType.Info);
            this.TempDataHelper.AddMessasge(msg);

            return View(viewName, null);
        }

        [Authorize]
        [HttpGet("registersuccess")]
        public IActionResult Success()
        {


            // will return to this endpoint
            const string viewName = Bom.Web.Identity.IdentityViewProvider.SharedOAuthRegistration; ; //  "~/Areas/Identity/Views/Shared/OAuthRegistration.cshtml";


            return View(viewName, null);


        }

        public IActionResult GetOAuthRegistrationSuccessView(OAuthRegVm oauthView)
        {
            const string viewName = Bom.Web.Identity.IdentityViewProvider.SharedOAuthRegistration; ; //  "~/Areas/Identity/Views/Shared/OAuthRegistration.cshtml";

            return View(viewName, oauthView);
        }



    }
}