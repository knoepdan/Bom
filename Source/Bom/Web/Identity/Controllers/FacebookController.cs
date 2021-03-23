﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ch.Knomes.Security;
using Bom.Core.Common;
using Bom.Web.Nodes.Models;
using Bom.Web.Common.Infrastructure;
using Bom.Web.Identity.Models;
using Bom.Core.Identity;
using Bom.Core.Identity.DbModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization;
using Bom.Web.Common;

namespace Bom.Web.Identity.Controllers
{
    [Area("Identity")]
  //  [Route("facebook")]
    [Route("/{" + Const.RouteArgumentNames.Lang + "}/{" + Const.RouteArgumentNames.Controller + "}")]
    [Controller]
    public class FacebookController : OAuthBaseController
    {
        public override string ProviderName => "Facebook";

        public FacebookController(ModelContext context) : base(context)
        {
    //        _context = context;
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register(object dummy)
        {
            if (!this.ModelState.IsValid)
            {
                throw new Exception("invalid model state");
            }
            var actionResult = await GoToSecondRegisterPage();
            return actionResult;
        }

        [Authorize]
        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {
            // TODO try to find a way prevent dummy request
            if (!this.ModelState.IsValid)
            {
                throw new Exception("invalid model state");
            }

            // will return to this endpoint
            var actionResult = await GoToSecondRegisterPage();
            return actionResult;
        }

        private async Task<IActionResult> GoToSecondRegisterPage()
        {
            var oAuthInfo = Bom.Web.Identity.IdentityHelper.GetFacebookType(this.User.Identities);
            if (oAuthInfo != null)
            {
                var linkProvider = new IdentityLinkProvider(this);

                var existingId = await this.Context.Users.Where(x => x.FacebookId == oAuthInfo.Identifier).Select(x => x.FacebookId).FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(existingId))
                {
                    return Redirect(linkProvider.OAuthRegisterAlreadyRegisteredLink(this.ProviderName));
                }

                var user = this.CreateNewOAuthUser(oAuthInfo);
                user.FacebookId = oAuthInfo.Identifier;
                this.Context.Users.Add(user);
                await this.Context.SaveChangesAsync();

                // redirect to
                var regVm = new OAuthRegVm(user, "Facebook");

                return Redirect(linkProvider.OAuthRegisterAlreadyRegisteredLink(this.ProviderName));//RedirectToAction("success");
            }
            else
            {
                throw new Exception("Identities not found");  // or just a redirect to normal register??
            }
        }

        

    }
}