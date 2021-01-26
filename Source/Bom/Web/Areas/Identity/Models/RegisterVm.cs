using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Ch.Knomes.ComponentModel.DataAnnotations;

namespace Bom.Web.Areas.Identity.Models
{
    public class RegisterVm
    {


        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Username { get; set; } = default!;

        // [Required]
        [ExtRequired(ErrorMessageResourceName = "MVC.Validation.Required")]
        [Display(Name ="TODO - also provide localization support")]
        [StringLength(255)]
        public string Password { get; set; } = default!;

        [Required]
        public string ConfirmPassword { get; set; } = default!;
    }
}
