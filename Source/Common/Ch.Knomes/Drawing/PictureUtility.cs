using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Text;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.MetaData;
//using SixLabors.ImageSharp.MetaData.Profiles;

namespace Ch.Knomes.Drawing
{
    public enum ImageFormat
    {
        Jpeg = 0,
        Png = 1,
        Gif = 2
    }

    public static class PictureUtility
    {

        public static bool IsImage(string fullFilePath)
        {
            if (fullFilePath == null)
            {
                throw new ArgumentNullException(nameof(fullFilePath));
            }
            // not the most elegant solution: http://stackoverflow.com/questions/670546/determine-if-file-is-an-image
            try
            {
                using (Image image = Image.Load(fullFilePath))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Expected ex in IsImage: " + ex.Message);
                return false;
            }
        }

        public static string GetMimeContentString(ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Gif:
                    return "image/gif";
                case ImageFormat.Jpeg:
                    return "image/jpeg";
                case ImageFormat.Png:
                    return "image/png";
                default:
                    throw new Exception("Unknown format to get content string: " + format); // should never happen
            }
        }

        public static ImageFormat GetImageFormatFromName(string fileName, ImageFormat? fallback)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("passed filename may not be empty", nameof(fileName));
            }
            ImageFormat? foundFormat = null;
            if (Path.HasExtension(fileName))
            {
                var ext = Path.GetExtension(fileName).Trim().ToUpperInvariant().TrimStart('.');
                switch (ext)
                {
                    case "JPG":
                    case "JPEG":
                        foundFormat = ImageFormat.Jpeg;
                        break;
                    case "PNG":
                        foundFormat = ImageFormat.Png;
                        break;
                    case "GIF":
                        foundFormat = ImageFormat.Gif;
                        break;
                    default:
                        // do nothing
                        break;
                }
            }

            if (!foundFormat.HasValue)
            {
                if (!fallback.HasValue)
                {
                    throw new Exception($"Could not determine the imageformat from the following filename: '{fileName}' and no fallback format was passed");
                }
                foundFormat = fallback;
            }
            return foundFormat.Value;
        }

        public static MemoryStream ResizeImage(Stream imageStream, int targetWith, int targetHeight, ImageFormat targetFormat)
        {
            if (imageStream == null)
            {
                throw new ArgumentNullException(nameof(imageStream));
            }
            if (imageStream.Length == 0)
            {
                throw new ArgumentException("length of passed stream needs to be bigger than 0", nameof(imageStream));
            }
            var targetStream = new MemoryStream();
            using (var image = Image.Load(imageStream))
            {
                image.Mutate(x => x
                     .Resize(targetWith, targetHeight)
                     .Grayscale());
                SaveImageToStream(image, targetFormat, targetStream);
            }
            return targetStream;
        }

        public static void ResizeImage(string imagePath, string targetPath, int maxWith, int maxHeight)
        {
            if (!System.IO.File.Exists(imagePath))
            {
                throw new ArgumentException("Passed source image does not exist: " + imagePath, imagePath);
            }
            using (var image = Image.Load(imagePath))
            {
                image.Mutate(x => x
                     .Resize(maxWith, maxHeight)
                     .Grayscale());
                //image.Mutate(x => x
                //     .Resize(image.Width / 2, image.Height / 2)
                //     .Grayscale());

                image.Save(targetPath); // Automatic encoder selected based on extension.
            }
        }

        //private static void SaveImageToStream<TPixel>(Image img, ImageFormat format, Stream targetStream) where TPixel : struct, IPixel
       private static void SaveImageToStream(Image img, ImageFormat format, Stream targetStream)
        {
            switch (format)
            {
                case ImageFormat.Gif:
                    img.SaveAsGif(targetStream);
                    break;
                case ImageFormat.Jpeg:
                    img.SaveAsJpeg(targetStream);
                    break;
                case ImageFormat.Png:
                    img.SaveAsPng(targetStream);
                    break;
                default:
                    throw new Exception("Unknown format: " + format); // should never happen
            }
        }
    }
}
