﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ch.Knomes.Security;
using Bom.Core.Common;
using Bom.Web.Areas.Main.Models;
using Bom.Web.Lib.Infrastructure;
using Bom.Web.Areas.Identity.Models;
using Bom.Core.Identity;
using Bom.Core.Identity.DbModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization;

namespace Bom.Web.Areas.Main.Controllers
{
    [Area("Identity")]
    [Route("facebook")]
    [Controller]
    public class FacebookController : OAuthBaseController
    {
        //private readonly ModelContext _context;

        public FacebookController(ModelContext context) : base(context)
        {
    //        _context = context;
        }

        //// GET: api/Paths
        //[HttpGet("register")]
        //public IActionResult Register()
        //{
        //    var model = new RegisterVm();

        //    return View( model);
        //    //  return View("~/Areas/Identity/Views/Account/Register", model);
        //}

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register(object dummy)
        {
            if (this.ModelState.IsValid)
            {
                throw new Exception("invalid model state");
            }


            var actionResult = await GoToSecondRegisterPage();
            return actionResult;
        }

       // [Authorize]
        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {
            // TODO try to find a way prevent dummy request
            if (this.ModelState.IsValid)
            {
                throw new Exception("invalid model state");
            }

            // will return to this endpoint


            var actionResult = await GoToSecondRegisterPage();
            return actionResult;


        }


        private async Task<IActionResult> GoToSecondRegisterPage()
        {
            var oAuthInfo = Bom.Web.Areas.Identity.IdentityHelper.GetFacebookType(this.User.Identities);
            if (oAuthInfo != null)
            {
     

                var existingId = await this.Context.Users.Where(x => x.FacebookId == oAuthInfo.Identifier).Select(x => x.FacebookId).FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(existingId))
                {
                    throw new Exception("Already exists");
                }

                var user = this.CreateNewOAuthUser(oAuthInfo);
                user.FacebookId = oAuthInfo.Identifier;
                this.Context.Users.Add(user);
                await this.Context.SaveChangesAsync();



                // redirect to
                var regVm = new OAuthRegVm(user, "Facebook");


                return this.View(regVm);
            }
            else
            {
                throw new Exception("Identities not found");  // or just a redirect to normal register??
            }
        }
    }
}