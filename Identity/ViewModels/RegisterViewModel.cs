using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]        
        [Display(Name = "Username")]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 4, ErrorMessage ="Username is invalid")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage =
            "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage =
            "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // Add the new address properties:
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }

        //// Use a sensible display name for views:
        //[Display(Name = "Postal Code")]
        //public string PostalCode { get; set; }
    }
}
