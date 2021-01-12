using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bom.Web.Areas.Identity.Models
{
    public class RegisterVm
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Username { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string? Password { get; set; } = "";

        [Required]
        public string? ConfirmPassword { get; set; } = "";
    }
}
