using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Http;


namespace Bom.Web.Common.Controllers.Upload
{
    public interface IPostSaveHandler
    {
        string TreatUploadAfterSaveHandler(string fullFilePath, HttpRequest request, bool isImage);
    }
}
