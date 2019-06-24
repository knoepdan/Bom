using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Web.Controllers.Upload
{
    public class UploadOnlyWhenAuthenticated : IUploadChecker
    {
        public bool CanPerformUpload(string passedFileName, HttpRequest request, System.Security.Principal.IPrincipal currentUser, bool isImage)
        {
            bool ok = (currentUser != null && currentUser.Identity.IsAuthenticated);
            return ok;
        }

        public bool CanSeeTempUpload(string passedFileName, HttpRequest request, System.Security.Principal.IPrincipal currentUser, bool isImage)
        {
            bool ok = (currentUser != null && currentUser.Identity.IsAuthenticated);
            return ok;
        }
    }
}
