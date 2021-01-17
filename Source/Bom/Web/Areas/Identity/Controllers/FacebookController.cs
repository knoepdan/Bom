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

namespace Bom.Web.Areas.Main.Controllers
{
    [Area("Identity")]
    [Route("facebook")]
    [Controller]
    public class FacebookController : BomBaseViewController
    {
        private readonly ModelContext _context;

        public FacebookController(ModelContext context)
        {
            _context = context;
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
        public IActionResult Register(object dummy)
        {
            var oAuthInfo = Bom.Web.Areas.Identity.IdentityHelper.GetFacebookType(this.User.Identities);
            if (oAuthInfo != null)
            {
                // TODO check if identity is already taken
                // TODO2 if no create user and safe (or best forward to another page to confirm etc. (and potentially change username emailaddress) 

                this.GoToSecondRegisterPage();




            }



            // TODO
            if (this.ModelState.IsValid)
            {
                // TODO -> validate confirm pw, validate if email is unique and return proper error message
                /*
                var pwHelper = new PasswordHelper();
                var pwResult = pwHelper.HashPasswordWithRandomSalt(model.Password, true);

                var user = new User();
                user.Username = model.Username;
                user.Salt = pwResult.SaltString;
                user.PasswordHash = pwResult.HashString;
                user.ActivationToken = Guid.NewGuid() + DateTime.Now.Ticks.ToString() + CryptoUtility.GetPseudoRandomString(5);
                this._context.Users.Add(user);
                await this._context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
                */
            }
            return View();
            //  return View("~/Areas/Identity/Views/Account/Register", model);
        }

       // [Authorize]
        [HttpGet("register")]
        public IActionResult Register()
        {
            // will return to this endpoint

            var oAuthInfo = Bom.Web.Areas.Identity.IdentityHelper.GetFacebookType(this.User.Identities);
            if (oAuthInfo != null)
            {
                // TODO check if identity is already taken
                // TODO2 if no create user and safe (or best forward to another page to confirm etc. (and potentially change username emailaddress) 

                this.GoToSecondRegisterPage();
            }        
            else
            {
                throw new Exception("Did not authentication to facebook");
            }


            return View();

        }


        private void GoToSecondRegisterPage()
        {
            // TODO
        }
    }
}