using System;
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

using Bom.Web.Areas.Identity;

namespace Bom.Web.Areas.Main.Controllers
{
  //  [Area("Identity")]
  //  [Route("facebook")]
    [Controller]
    public abstract class OAuthBaseController : BomBaseViewController
    {
        protected ModelContext Context { get; }

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


        public IActionResult GetFirstStep()
        {
            return View("OAuthRegistration");
        }


        //// GET: api/Paths
        //[HttpGet("register")]
        //public IActionResult Register()
        //{
        //    var model = new RegisterVm();

        //    return View( model);
        //    //  return View("~/Areas/Identity/Views/Account/Register", model);
        //}

    }
}