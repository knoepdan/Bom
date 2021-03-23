using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ch.Knomes.Security;
using Bom.Core.Common;
using Bom.Web.Common.Infrastructure;
using Bom.Web.Identity.Models;
using Bom.Core.Identity;
using Bom.Core.Identity.DbModels;
using Bom.Web.Common;
using Bom.Web.Identity.Controllers;


namespace Bom.Web.Identity
{
    public class IdentityLinkProvider
    {
        public IdentityLinkProvider(IUrlHelper urlHelper)
        {
            this._urlHelper = urlHelper;
            this._langCode = GetLangCode();
        }

        public IdentityLinkProvider(IUrlHelper urlHelper, string currentLangCode)
        {
            this._urlHelper = urlHelper;
            this._langCode = currentLangCode;
        }

        public IdentityLinkProvider(BomBaseViewController controller)
        {
            this._urlHelper = controller.Url;
            this._langCode = GetLangCode();
        }

        private readonly IUrlHelper _urlHelper;

        private string _langCode = Const.DefaultLang;

        public string AccountRegisterLink => this._urlHelper.Content($"~/{this._langCode}/account/register")!;

        public string AccountLoginLink => this._urlHelper.Content($"~/{this._langCode}/account/login")!;


        public string AccountLogoutLink => this._urlHelper.Content($"~/{this._langCode}/account/logout")!;

        public string FacebookRegisterLink => this._urlHelper.Content($"~/{this._langCode}/facebook/register")!;

        public string OAuthRegisterAlreadyRegisteredLink(string providerName) => this._urlHelper.Content($"~/{this._langCode}/{providerName.ToLowerInvariant()}/alreadyregistered")!;


        public string OAuthRegisterSuccessLink(string providerName) => this._urlHelper.Content($"~/{this._langCode}/{providerName.ToLowerInvariant()}/registersuccess")!;


        private string GetLangCode()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }
    }
}
