namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        PublicID = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false, maxLength: 25),
                        ProjectType = c.String(nullable: false, maxLength: 50),
                        ProjectDescription = c.String(nullable: false, maxLength: 250),
                        DateCreated = c.DateTime(nullable: false, storeType: "date"),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.PublicID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID, cascadeDelete: true)
                .Index(t => t.ApplicationUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ApplicationUserID" });
            DropTable("dbo.Projects");
        }
    }
}
