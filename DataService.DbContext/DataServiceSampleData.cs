using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Diagnostics;

namespace twg.chk.DataService.DbContext
{
    public class DataServiceSampleData : DropCreateDatabaseIfModelChanges<DataServiceEntities>
    {
        private UserManager<IdentityUser> _userManager;
        public DataServiceSampleData(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        protected override void Seed(DataServiceEntities context)
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
        }
    }
}
