using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Ch.Knomes.Localization.DataAnnotations;

namespace Bom.Web.Identity.Models
{
    public class RegisterVm
    {


        [LocRequired]
        [LocEmailAddress]
        [LocStringLength(255)]
        public string Username { get; set; } = default!;

        // [Required]
        [LocRequired(ErrorMessageResourceName = "MVC.Validation.Required")]
        [LocDisplayName("Password")]
        [LocStringLength(255)]
        public string Password { get; set; } = default!;

        [LocRequired]
        public string ConfirmPassword { get; set; } = default!;
    }
}
