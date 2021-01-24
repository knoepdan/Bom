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
using Bom.Web.Lib;


namespace Bom.Web.Areas.Identity
{
    public class IdentityLinkProvider
    {
        public IdentityLinkProvider(IUrlHelper urlHelper, Lib.Mvc.LayoutData layoutData)
        {
            this._urlHelper = urlHelper;
            this._langCode = layoutData.GetCurrentLangCode();
        }

        public IdentityLinkProvider(IUrlHelper urlHelper, string currentLangCode)
        {
            this._urlHelper = urlHelper;
            this._langCode = currentLangCode;
        }

        public IdentityLinkProvider(BomBaseViewController controller)
        {
            this._urlHelper = controller.Url;
            this._langCode = controller.CurrentLanguage;
        }

        private readonly IUrlHelper _urlHelper;

        private string _langCode = Const.DefaultLang;

        public string AccountRegisterLink => this._urlHelper.Content($"~/{this._langCode}/account/register")!;

        public string FacebookRegisterLink => this._urlHelper.Content($"~/{this._langCode}/facebook/register")!;


    }
}
