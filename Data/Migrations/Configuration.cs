namespace Data.Migrations
{
    using akcetDB;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Data.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            if (!(context.Users.Any(u => u.UserName == "svetlin.krastanov90@gmail.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser {
                    UserName = "svetlin.krastanov90@gmail.com",
                    PhoneNumber = "0888017004",
                    Email = "svetlin.krastanov90@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    DateCreated = DateTime.Now
                };
                userManager.Create(userToInsert, "svetlin90");
            }

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
