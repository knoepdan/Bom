using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace Bom.Web.Controllers.Upload
{

    public class EnsureWebImagesPostSaveHandler : IPostSaveHandler
    {
        public int ImageMaxWith { get; set; } = 0;

        public int ImageMaxHeight { get; set; } = 0;

        public bool KeepMetadata { get; set; }

        public string TreatUploadAfterSaveHandler(string fullFilePath, HttpRequest request, bool isImage)
        {
            string folderPath = Path.GetDirectoryName(fullFilePath);
            //string fileName = Ch.Knomes.Drawing.PictureMetadata.ResizeImage(fullFilePath, folderPath, ImageMaxWith, ImageMaxHeight, KeepMetadata); // converts to web-image (example: tiff -> jpeg, and slightly adapts ending in some cases)

            Utils.Dev.Todo("Keep metadata is not yet supported", Utils.Dev.Urgency.Middle);
            var size = new Size(ImageMaxWith, ImageMaxHeight);

            var targetPath = Path.Combine(folderPath, "xxx");
            Ch.Knomes.Drawing.PictureUtility.ResizeImage(fullFilePath, targetPath, size); // converts to web-image (example: tiff -> jpeg, and slightly adapts ending in some cases)
            return targetPath;
        }
    }
}
