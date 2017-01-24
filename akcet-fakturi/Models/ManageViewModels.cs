using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Tools.Attributes;

namespace akcet_fakturi.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string UserName { get; set; }
        public string BankAccount { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string Dds { get; set; }
        public string Kwk { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class ChangeNames
    {
        [Required]
        [MultiLanguageDisplayName("19")]
        public string FirstName { get; set; }

        [Required]
        [MultiLanguageDisplayName("20")]
        public string LastName { get; set; }
    }

    public class ChangeOtherInfoViewModel
    {
        [MultiLanguageDisplayName("1026")]
        public string Address { get; set; }

        [MultiLanguageDisplayName("1048")]
        public string ZipCode { get; set; }

        [MultiLanguageDisplayName("1049")]
        public string City { get; set; }

        [MultiLanguageDisplayName("1069")]
        public string CompanyName { get; set; }

        [MultiLanguageDisplayName("1067")]
        public string BankAccount { get; set; }

        [MultiLanguageDisplayName("1068")]
        public string KwkNumber { get; set; }

        [MultiLanguageDisplayName("1046")]
        public string DdsNumber { get; set; }

    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}