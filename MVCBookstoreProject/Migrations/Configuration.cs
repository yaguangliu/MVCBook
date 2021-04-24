using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCBookstoreProject.Models;

namespace MVCBookstoreProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCBookstoreProject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCBookstoreProject.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == RoleName.CanManage))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole {Name = RoleName.CanManage};
                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                    {UserName = "admin@admin.com", FirstName = "Admin", LastName = "", Email = "admin@admin.com"};
                manager.Create(user, "=CanManage");
                manager.AddToRole(user.Id, RoleName.CanManage);
            }
        }
    }
}
