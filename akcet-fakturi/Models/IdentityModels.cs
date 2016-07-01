using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace akcet_fakturi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилно име")]
        public string LastName { get; set; }

        [Display(Name = "Банкова сметка")]
        public string BankAcount { get; set; }

        [Display(Name = "КВК номер")]
        public string KwkNumber { get; set; }

        [Display(Name = "ДДС номер")]
        public string DdsNumber { get; set; }

        [Display(Name = "Компания")]
        public string CompanyName { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Пощенски код")]
        public string ZipCode { get; set; }
        
        [StringLength(500)]
        [Display(Name = "Град")]
        public string City { get; set; }


        [Display(Name = "Регистриран на")]
        public DateTime DateCreated { get; set; }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<akcetDB.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<akcetDB.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<akcetDB.Product> Products { get; set; }

        public System.Data.Entity.DbSet<akcetDB.DD> DDs { get; set; }
        public System.Data.Entity.DbSet<akcetDB.Fakturi> Fakturis { get; set; }

        public System.Data.Entity.DbSet<akcetDB.Project> Projects { get; set; }
    }
}