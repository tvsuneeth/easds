namespace twg.chk.DataService.DbContext.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Diagnostics;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class Configuration : DbMigrationsConfiguration<twg.chk.DataService.DbContext.DataServiceEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "twg.chk.DataService.DbContext.DataServiceEntities";
        }

        protected override void Seed(twg.chk.DataService.DbContext.DataServiceEntities context)
        {
            var backOfficeGroup = context.Roles.SingleOrDefault(r => r.Name.Equals("backofficegroup"));
            if (backOfficeGroup == null)
            {
                backOfficeGroup = new IdentityRole { Name = "backofficegroup" };
                context.Roles.Add(backOfficeGroup);
                context.Commit();
                Debug.WriteLine("Created backofficegroup role");
            }

            var frontofficegroup = context.Roles.SingleOrDefault(r => r.Name.Equals("frontofficegroup"));
            if (frontofficegroup == null)
            {
                frontofficegroup = new IdentityRole { Name = "frontofficegroup" };
                context.Roles.Add(frontofficegroup);
                context.Commit();
                Debug.WriteLine("Created frontofficegroup role");
            }

            var user = context.Users.SingleOrDefault(u => u.UserName.Equals("backofficeadmin"));
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "backofficeadmin",
                    PasswordHash = new PasswordHasher().HashPassword("Passw0rd"),
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                context.Users.Add(user);
                context.Commit();
                Debug.WriteLine("Created backofficeadmin user");

                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = backOfficeGroup.Id });
                context.Commit();
                Debug.WriteLine("Added to backofficegroup role backofficeadmin user");

                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = frontofficegroup.Id });
                context.Commit();
                Debug.WriteLine("Added to frontofficegroup role backofficeadmin user");
            }

            var user2 = context.Users.SingleOrDefault(u => u.UserName.Equals("frontofficeuser"));
            if (user2 == null)
            {
                user2 = new IdentityUser
                {
                    UserName = "frontofficeuser",
                    PasswordHash = new PasswordHasher().HashPassword("Passw0rd"),
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                context.Users.Add(user2);
                context.Commit();
                Debug.WriteLine("Created frontofficeuser user");

                user2.Roles.Add(new IdentityUserRole { UserId = user2.Id, RoleId = frontofficegroup.Id });
                context.Commit();
                Debug.WriteLine("Added to frontofficegroup role frontofficeuser user");
            }
        }
    }
}
