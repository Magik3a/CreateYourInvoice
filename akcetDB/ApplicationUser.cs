using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Tools.Attributes;

namespace akcetDB
{
    public class ApplicationUser : IdentityUser
    {
        [MultiLanguageDisplayName("19")]
        public string FirstName { get; set; }

        [MultiLanguageDisplayName("20")]
        public string LastName { get; set; }

        [MultiLanguageDisplayName("1067")]
        public string BankAcount { get; set; }

        [MultiLanguageDisplayName("1068")]
        public string KwkNumber { get; set; }

        [MultiLanguageDisplayName("1046")]
        public string DdsNumber { get; set; }

        [MultiLanguageDisplayName("1069")]
        public string CompanyName { get; set; }

        [MultiLanguageDisplayName("1026")]
        public string Address { get; set; }

        [MultiLanguageDisplayName("1048")]
        public string ZipCode { get; set; }

        [StringLength(500)]
        [MultiLanguageDisplayName("1049")]
        public string City { get; set; }


        [MultiLanguageDisplayName("1070")]
        public DateTime DateCreated { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
