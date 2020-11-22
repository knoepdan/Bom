using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
//using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.Metadata.Profiles;

namespace Ch.Knomes.Drawing
{
    /// <summary>
    /// Simplifies Access to the most common Image Metadata 
    /// </summary>
    public sealed class PictureMetadata : IDisposable
    {

        // Image useing ImageSharp
        //https://github.com/SixLabors/ImageSharp
        // https://www.hanselman.com/blog/HowDoYouUseSystemDrawingInNETCore.aspx

        // image processing with old .Net
        // inspired by: http://stackoverflow.com/questions/4983766/getting-gps-data-from-an-images-exif-in-c-sharp
        // also interesting: http://stackoverflow.com/questions/20801314/how-to-fetch-the-geotag-details-of-the-captured-image-or-stored-image-in-windows  (but not used)
        // for testing: http://regex.info/exif.cgi

        public PictureMetadata(string pathToImage) : this(new FileInfo(pathToImage)) { }

        public PictureMetadata(FileInfo fileInfo)
        {
            if (fileInfo == null) { throw new ArgumentNullException(nameof(fileInfo)); }
            if (!fileInfo.Exists) { throw new ArgumentException($"File does not exist at '{fileInfo.FullName}'", nameof(fileInfo)); }

            var path = fileInfo.FullName;
            this.Image = SixLabors.ImageSharp.Image.Load(path);
            this.MetaData = this.Image.Metadata;

        }

        internal Image Image { get; }

        internal ImageMetadata MetaData { get; }

        public int Height => Image.Height;

        public int Width => Image.Width;


        public DateTime? DateTaken
        {
            get
            {
                var toReturn = GetDateTaken(MetaData, SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.DateTime);
                return toReturn;
            }
        }

        public float? Latitude
        {
            get
            {
                var toReturn = GetPositionFromMeta(this.MetaData, SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.GPSLatitude, SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.GPSLatitudeRef);
                return toReturn;
            }
        }

        public float? Longitude
        {
            get
            {
                var toReturn = GetPositionFromMeta(this.MetaData, SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.GPSLongitude, SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.GPSLongitudeRef);
                return toReturn;
            }
        }

#pragma warning disable CA1801 // Review unused parameters
        private static float? GetPositionFromMeta(ImageMetadata metaData, SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag positionField, SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag positionRefField)
#pragma warning restore CA1801 // Review unused parameters
        {
            try
            {
                throw new NotImplementedException($"{nameof(GetPositionFromMeta)} is not implemented after upgrading");
                /*
                var posField = metaData.ExifProfile.GetValue(positionField);
                if (posField != null && posField.IsArray && posField.DataType == SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifDataType.Rational)
                {
                    var posValues = (SixLabors.ImageSharp.Primitives.Rational[])posField.Value;
                    SixLabors.ImageSharp.MetaData.Profiles.Exif.ExifValue? refField = metaData.ExifProfile.GetValue(positionRefField);
                    string? metaRefVal = refField != null && refField.DataType == SixLabors.ImageSharp.MetaData.Profiles.Exif.ExifDataType.Ascii ? refField.Value as string : "";
                    var floatVal = ExifGpsToFloat(posValues, metaRefVal);
                    return floatVal;
                }
                */
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /*
        private static float ExifGpsToFloat(SixLabors.ImageSharp.Primitives.Rational[] propItem, string? propItemRef)
        {
            uint degreesNumerator = propItem[0].Numerator;//BitConverter.ToUInt32(propItem.Value, 0);
            uint degreesDenominator = propItem[0].Denominator;//BitConverter.ToUInt32(propItem.Value, 4);
            float degrees = degreesNumerator / (float)degreesDenominator;

            uint minutesNumerator = propItem[1].Numerator;//BitConverter.ToUInt32(propItem.Value, 8);
            uint minutesDenominator = propItem[1].Denominator;//BitConverter.ToUInt32(propItem.Value, 12);
            float minutes = minutesNumerator / (float)minutesDenominator;

            uint secondsNumerator = propItem[2].Numerator;//BitConverter.ToUInt32(propItem.Value, 16);
            uint secondsDenominator = propItem[2].Denominator; //BitConverter.ToUInt32(propItem.Value, 20);
            float seconds = secondsNumerator / (float)secondsDenominator;

            float coorditate = degrees + (minutes / 60f) + (seconds / 3600f);
            if (!string.IsNullOrEmpty(propItemRef)) // //N, S, E, or W
            {
                var gpsRef = propItemRef.Trim().ToUpperInvariant();
                if (gpsRef == "S" || gpsRef == "W")
                {
                    coorditate = 0 - coorditate;
                }
            }
            return coorditate;
        }
        */

        private static DateTime? GetDateTaken(ImageMetadata metaData, SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag dateField)
        {
            throw new NotImplementedException($"{nameof(GetDateTaken)} not implemented after upgrading to Nert 5");
            /*try
            {
                var dField = metaData.ExifProfile.GetValue(dateField);
                if (dField == null || dField.DataType != SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifDataType.Ascii)
                {
                    return null;
                }

                //Convert date taken metadata to a DateTime object
                string sdate = ((string)dField.Value).Trim();//propItem.Value;///Encoding.UTF8.GetString(propItem.Value).Trim();
                string secondhalf = sdate.Substring(sdate.IndexOf(" ", StringComparison.InvariantCulture), (sdate.Length - sdate.IndexOf(" ", StringComparison.InvariantCulture)));
                string firsthalf = sdate.Substring(0, 10);
                firsthalf = firsthalf.Replace(":", "-", StringComparison.InvariantCulture);
                sdate = firsthalf + secondhalf;
                var dateTaken = DateTime.Parse(sdate, System.Globalization.CultureInfo.InvariantCulture);
                return dateTaken;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message); ///dtaken = DateTime.Parse("1956-01-01 00:00:00.000");
                return null;
            }
            */
        }

        public void Dispose()
        {
            this.Image.Dispose();
        }
    }
}