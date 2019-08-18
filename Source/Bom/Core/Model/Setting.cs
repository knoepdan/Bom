using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Bom.Core.Model
{
    /// <summary>
    /// Setting
    /// </summary>
    public class Setting
    {
        public int SettingId { get; private set; }

        [Required]
        [StringLength(255)]
        public string Key { get; set; }

        [Required]
        [StringLength(255)]
        public string Value { get; set; }
    }
}
