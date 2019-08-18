using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Bom.Core.Model
{
    /// <summary>
    /// Data to be saved in Db (example: image)
    /// </summary>
    /// <remarks>Own data structure to facilitate lazy load properties</remarks>
    public class BlobData
    {
        public int BlobDataId { get; private set; }

        [Required]
        public byte[] Data { get; set; }

    }
}
