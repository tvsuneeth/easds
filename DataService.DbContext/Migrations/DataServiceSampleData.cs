using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Diagnostics;
using System.Data.Entity.Migrations;

using twg.chk.DataService.Business;

namespace twg.chk.DataService.DbContext.Migrations
{
    public class DataServiceSampleDataConfiguration : Configuration
    {
        public DataServiceSampleDataConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "twg.chk.DataService.DbContext.DataServiceEntities";
        }

        protected override void Seed(DataServiceEntities context)
        {
            base.Seed(context);

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

            // Seed static content link **************************************************************
            var disclaimer = context.StaticContentLinks.SingleOrDefault(s => s.IdentificationValue.Equals("disclaimer"));
            if (disclaimer == null)
            {
                context.StaticContentLinks.Add(new StaticContentLink { IdentificationValue = "disclaimer", LinkType = StaticLinkType.StaticPage, Title = "Disclaimer Static Page" });
            }

            var restaurants = context.StaticContentLinks.SingleOrDefault(s => s.IdentificationValue.Equals("restaurants"));
            if (restaurants == null)
            {
                context.StaticContentLinks.Add(new StaticContentLink { IdentificationValue = "restaurants", LinkType = StaticLinkType.Sector, Title = "Restaurants" });
            }

            var pubsAndBars = context.StaticContentLinks.SingleOrDefault(s => s.IdentificationValue.Equals("pubs & bars"));
            if (pubsAndBars == null)
            {
                context.StaticContentLinks.Add(new StaticContentLink { IdentificationValue = "pubs & bars", LinkType = StaticLinkType.Sector, Title = "Pubs & Bars" });
            }

            var foodService = context.StaticContentLinks.SingleOrDefault(s => s.IdentificationValue.Equals("food service"));
            if (foodService == null)
            {
                context.StaticContentLinks.Add(new StaticContentLink { IdentificationValue = "food service", LinkType = StaticLinkType.Sector, Title = "Food Service" });
            }

            var hotels = context.StaticContentLinks.SingleOrDefault(s => s.IdentificationValue.Equals("hotels"));
            if (hotels == null)
            {
                context.StaticContentLinks.Add(new StaticContentLink { IdentificationValue = "hotels", LinkType = StaticLinkType.Sector, Title = "Hotels" });
            }

            context.Commit();
        }
    }

    public class DataServiceSampleData : MigrateDatabaseToLatestVersion<DataServiceEntities, DataServiceSampleDataConfiguration>
    {
        public DataServiceSampleData() {}
    }
}
