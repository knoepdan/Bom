using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Bom.Core.Identity.DbModels;

namespace Bom.Web.Areas.Identity.Models
{
    public class OAuthRegVm
    {
        public OAuthRegVm() { }


        public OAuthRegVm(User user, string providerName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            this.ProviderName = providerName;
            Username = user.Username;
            Name = user.Name;
        }

        public string ProviderName { get; set; } = "?";


        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Username { get; set; } = default!;

        [Required]
        public string Name { get; set; } = "";


        [StringLength(255)]
        public string Password { get; set; } = default!;

        public string ConfirmPassword { get; set; } = default!;



    }
}
