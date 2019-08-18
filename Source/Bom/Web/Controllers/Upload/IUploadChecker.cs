using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bom.Web.Controllers.Upload
{
    public interface IUploadChecker
    {
        bool CanPerformUpload(string passedFileName, HttpRequest request, System.Security.Principal.IPrincipal currentUser, bool isImage);

        bool CanSeeTempUpload(string passedFileName, HttpRequest request, System.Security.Principal.IPrincipal currentUser, bool isImage);
    }
}
