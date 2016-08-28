using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using akcetDB;

namespace Data
{

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
        public System.Data.Entity.DbSet<akcetDB.Counter> Counters { get; set; }

        public System.Data.Entity.DbSet<akcetDB.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<akcetDB.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<akcetDB.Product> Products { get; set; }

        public System.Data.Entity.DbSet<akcetDB.DD> DDs { get; set; }
        public System.Data.Entity.DbSet<akcetDB.Fakturi> Fakturis { get; set; }

        public System.Data.Entity.DbSet<akcetDB.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<akcetDB.FakturiTemp> FakturiTemp { get; set; }

        public System.Data.Entity.DbSet<akcetDB.ProductInvoice> ProductInvoices { get; set; }
        public System.Data.Entity.DbSet<akcetDB.ProductInvoiceTemp> ProductInvoicesTemp { get; set; }


        public System.Data.Entity.DbSet<akcetDB.Werkbrief> Werkbriefs { get; set; }

        public System.Data.Entity.DbSet<akcetDB.WerkbriefTemp> WerkbriefTemps { get; set; }

        public System.Data.Entity.DbSet<akcetDB.WerkbriefHours> WerkbriefHours { get; set; }


        public System.Data.Entity.DbSet<akcetDB.WerkbriefHoursTemp> WerkbriefHoursTemps { get; set; }




    }
}
