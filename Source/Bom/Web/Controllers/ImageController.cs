using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Data;
using Bom.Core.Model;
using Ch.Knomes.Drawing;
using Bom.Web.Lib.Infrastructure;

namespace Bom.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : BomBaseController
    {
        private const ImageFormat FallBackFormat = ImageFormat.Jpeg;

        private readonly ModelContext _context;


        public ImageController(ModelContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ResponseCache(VaryByQueryKeys = new[] { "uid" }, Duration = 99000, Location = ResponseCacheLocation.Client)]
        public ActionResult Orig(string uid)
        {
            // maybe response caching has to be activated during startup   https://docs.microsoft.com/en-us/aspnet/core/performance/caching/middleware?view=aspnetcore-2.2

            var emptySize = new Size();
            var result = GetImage(uid, emptySize);

            SetDefaultImageHeaders(Response);
            return result;
        }

        private ActionResult GetImage(string uid, Size size)
        {
            Utils.Dev.PossibleImprovment(" performance improvments still possible (minimize db requests etc)", Utils.Dev.ImproveArea.Performance);

            // http://stackoverflow.com/questions/994135/image-from-httphandler-wont-cache-in-browser
            var isModifiedSinceFlag = this.Request.Headers["If-Modified-Since"];
            if (isModifiedSinceFlag.Any())
            {
                Response.StatusCode = 304; // Replace .AddHeader
                return new EmptyResult();// Content("");
            }

            //DbPicture pic = ModelContext.DbSet<DbPicture>().FirstOrDefault(x => x.DbPictureId == id);
            //if (pic == null)
            //{
            //    return new HttpNotFoundResult(""); // Response.StatusCode = 404;
            //}
            ImageCacheDto? imageBlob = GetFromCache(uid, size);
            if (imageBlob == null)
            {
                DbPicture? pic = this._context.DbPictures?.SingleOrDefault(x => x.PictureUid == uid);
                if (pic == null)
                {
                    return NotFound();
                    //  return new HttpNotFoundResult(""); // Response.StatusCode = 404;
                }
                imageBlob = new ImageCacheDto(pic);
                if (!size.IsEmpty)
                {
                    imageBlob.ImageBytes = GetResizedImageBytes(pic, size);
                }
                StoreInCache(pic.PictureUid, size, imageBlob);
            }
            string picName = imageBlob.ImageName;
            var imageFormat = PictureUtility.GetImageFormatFromName(picName, FallBackFormat);
            var mimeType = PictureUtility.GetMimeContentString(imageFormat);//MimeMapping.GetMimeMapping(picName); // "image/jpeg"
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = picName,
                Inline = true,
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");
            return new FileContentResult(imageBlob.ImageBytes, mimeType);
        }

        //private static byte[] ImageToByte(Image img)
        //{
        //    var converter = new ImageConverter();
        //    return (byte[])converter.ConvertTo(img, typeof(byte[]));
        //}

        private static byte[] GetResizedImageBytes(DbPicture dbPic, Size size)
        {
            var maxSize = new Size(size.Width == 0 ? dbPic.Width : size.Width, size.Height == 0 ? dbPic.Height : size.Height);
            var origSize = new Size(dbPic.Width, dbPic.Height);
            var targetSize = GeometryUtility.ShrinkToFit(origSize, maxSize);
            var imgFormat = PictureUtility.GetImageFormatFromName(dbPic.Name, FallBackFormat);
            if (dbPic.BlobData == null || dbPic.BlobData.Data == null)
            {
                return Array.Empty<byte>();
            }
            using (var origImage = new System.IO.MemoryStream(dbPic.BlobData.Data))
            {
                using (var resizedImage = PictureUtility.ResizeImage(origImage, targetSize.Width, targetSize.Height, imgFormat))
                {
                    var resizedByteArray = resizedImage.ToArray();
                    return resizedByteArray;
                }
            }
        }

        #region cache (not finished... TODO)

        private ImageCacheDto? GetFromCache(string? picId, Size targetSize)
        {
            Utils.Dev.Todo($"Implement cache for pics. Passed id: {picId}, size: {targetSize}  (contr: {this})");
            // https://www.c-sharpcorner.com/article/asp-net-core-2-0-caching/ 
            return null;
            //string key = GetKey(picId, targetSize);
            //var result = HttpContext.Cache[key] as ImageCacheDto;
            //return result;
        }

        private void StoreInCache(string picId, Size targetSize, ImageCacheDto dto)
        {
            string key = GetKey(picId, targetSize);
            Utils.Dev.Todo($"Implement cache store for pics. Passed id: {picId}, size: {targetSize}, dto: {dto} (contr: {this})");
            //  HttpContext.Cache[key] = dto;
        }

        private static string GetKey(string picId, Size targetSize)
        {
            var key = "img_" + picId;
            if (!targetSize.IsEmpty)
            {
                key += "_" + targetSize.Width + "_" + targetSize.Height;
            }
            return key;
        }
        [Serializable]
        internal class ImageCacheDto
        {
            public ImageCacheDto() { }
            public ImageCacheDto(DbPicture pic)
            {
                ImageName = pic.Name;
                Uid = pic.PictureUid;
                if (pic.BlobData != null && pic.BlobData.Data != null)
                {
                    ImageBytes = pic.BlobData.Data;
                }
            }

            public string ImageName { get; set; } = "";

#pragma warning disable CA1819 // Properties should not return arrays
            public byte[] ImageBytes { get; set; } = Array.Empty<byte>();
#pragma warning restore CA1819 // Properties should not return arrays

            public string Uid { get; set; } = "";
        }

        public static void SetDefaultImageHeaders(HttpResponse response)
        {
            Utils.Dev.Todo($"{nameof(SetDefaultImageHeaders)} is not implemented .. needed for caching! response is set: {response != null}");
            // to be set like this
            //response.Headers["Expires"] = "23"

            // old asp.net way
            //response.Cache.SetCacheability(HttpCacheability.Public);
            //response.Cache.SetExpires(DateTime.Now.AddDays(40));
            //// response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            //response.Cache.SetLastModifiedFromFileDependencies();

        }
        #endregion
    }
}