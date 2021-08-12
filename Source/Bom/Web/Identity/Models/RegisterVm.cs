using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Knomes.Localize.DataAnnotations;

namespace Bom.Web.Identity.Models
{
    public class RegisterVm
    {

        [LocDisplayName("E-Mail")]
        [LocRequired]
        [LocEmailAddress]
        [LocStringLength(255)]
        public string Username { get; set; } = default!;

        [LocDisplayName("Password")]
        [LocRequired(ErrorMessageResourceName = "MVC.Validation.Required")]
        [LocStringLength(255)]
        public string Password { get; set; } = default!;

        [LocDisplayName("Confirm Password")]
        [LocRequired]
        public string ConfirmPassword { get; set; } = default!;
    }
}
