using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Knomes.Localize.DataAnnotations;

namespace Bom.Web.Identity.Models
{
    public class LoginVm
    {


        [LocRequired]
        [LocStringLength(255)]
        [LocEmailAddress]
        public string Username { get; set; } = default!;

        // [Required]
        [LocRequired(ErrorMessageResourceName = "MVC.Validation.Required")]
        [LocDisplayName("Password")]
        [LocStringLength(255)]
        public string Password { get; set; } = default!;

        
        public string? TestField { get; set; } = default!;
    }
}
