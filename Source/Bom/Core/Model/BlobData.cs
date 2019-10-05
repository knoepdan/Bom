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
#pragma warning disable CA1819 // Properties should not return arrays
        public byte[] Data { get; set; } = Array.Empty<byte>();
#pragma warning restore CA1819 // Properties should not return arrays

    }
}
