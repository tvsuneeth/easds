namespace twg.chk.DataService.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStaticContentLink : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StaticContentLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LinkType = c.Int(nullable: false),
                        IdentificationValue = c.String(),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StaticContentLinks");
        }
    }
}
