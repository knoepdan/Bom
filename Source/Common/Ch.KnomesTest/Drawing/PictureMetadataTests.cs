using System;
using System.Collections.Generic;
using System.Text;
using Ch.Knomes.TestUtils.Resources;

namespace Ch.Knomes.Drawing
{
    using Xunit;
    public class ImageMetadataTests
    {
        [Fact]
        public void MetaData_of_picture_is_readable()
        {
            var imgPath = Resources.SmallPic;
            if (!System.IO.File.Exists(imgPath)){
                throw new Exception("file does not exist at location: " + imgPath);
            }
            // get metadata
            var metaDataOb = new PictureMetadata(imgPath);
            var longitude = metaDataOb.Longitude;
            var latitude = metaDataOb.Latitude;
            var date = metaDataOb.DateTaken;

            // assert
            Assert.NotNull(longitude);
            Assert.NotNull(latitude);
            Assert.True(longitude != latitude);
            Assert.NotNull(date);
            Assert.True(date.HasValue && date.Value > new DateTime(1990, 2, 4));
        }
    }
}