using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Bom.Core.Model
{
    public class DbPicture
    {
        /// <summary>
        /// Creates an instance of a DbPicture (including Metadata-Properties as far as known and possible)
        /// </summary>
        public static DbPicture CreateDbPicture(System.IO.FileInfo fileInfo)
        {
            if (fileInfo == null) { throw new ArgumentNullException(nameof(fileInfo)); }
            if (!fileInfo.Exists)
            {
                throw new ArgumentException("Cannot create DbPicture because file does not exist");
            }


            var dbPic = new DbPicture();
            dbPic.Name = fileInfo.Name;
            dbPic.BlobData = new BlobData();
            dbPic.BlobData.Data = System.IO.File.ReadAllBytes(fileInfo.FullName);
            dbPic.FileLenght = fileInfo.Length;

            // Metadata
            using (var imgMetadata = new Ch.Knomes.Drawing.PictureMetadata(fileInfo))
            {
                dbPic.Latitude = imgMetadata.Latitude;
                dbPic.Longitude = imgMetadata.Longitude;
                dbPic.TimeStamp = imgMetadata.DateTaken;

                dbPic.Height = imgMetadata.Height;
                dbPic.Width = imgMetadata.Width;
            }
            return dbPic;
        }


        public int DbPictureId { get; private set; }

        [StringLength(50)]
        public string PictureUid { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// DateTime when Picture was taken (UTC)
        /// </summary>
        public DateTime? TimeStamp { get; set; }

        public float? Longitude { get; set; }

        public float? Latitude { get; set; }

        /// <summary>
        /// Place 
        /// </summary>
        /// <remarks>Additional to Longitude/Latitude (for example name of the city, or nationalpark xxx), could be used for grouping etc.</remarks>
        [StringLength(255)]
        public string Place { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }


        /// <summary>
        /// File-length in bytes
        /// </summary>
        public long FileLenght { get; set; }

        #region  possible future propteries (outcommented)

        //public float? Altitude { get; set; }

        #endregion


        public int BlobDataId { get; set; }

        /// <summary>
        /// Actual Blob 
        /// </summary>
        [Required]
        public virtual BlobData BlobData { get; set; }


        public override string ToString()
        {
            return "DbPicture " + DbPictureId;
        }
    }
}
