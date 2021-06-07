using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bom.Core.Identity;

namespace Bom.Web.Common.Controllers
{
 //   [Route("api/[controller]")]
    [ApiController]
    public class BomBaseController : ControllerBase
    {
        public const string CurrentUserItemsKey = "cUI";

        /// <summary>
        /// User of current session (null if there is session is expired/invalid or has never existed)
        /// /// </summary>
        public IUser? CurrentUser
        {
            get
            {
                object? ob;
                if(this.HttpContext.Items.TryGetValue(CurrentUserItemsKey, out ob))
                {
                    return ob as IUser;
                }
                return null;
            }
        }
    }
}