using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
