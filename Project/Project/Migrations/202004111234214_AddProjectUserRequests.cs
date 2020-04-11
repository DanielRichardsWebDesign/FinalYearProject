namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectUserRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectUserRequests",
                c => new
                    {
                        ProjectUserRequestID = c.Int(nullable: false, identity: true),
                        PublicID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ProjectUserRequestID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Projects", t => t.PublicID, cascadeDelete: true)
                .Index(t => t.PublicID)
                .Index(t => t.ApplicationUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectUserRequests", "PublicID", "dbo.Projects");
            DropForeignKey("dbo.ProjectUserRequests", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectUserRequests", new[] { "ApplicationUserID" });
            DropIndex("dbo.ProjectUserRequests", new[] { "PublicID" });
            DropTable("dbo.ProjectUserRequests");
        }
    }
}
