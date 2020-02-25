namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileID = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        FileType = c.String(nullable: false),
                        FileSize = c.String(nullable: false),
                        FilePath = c.String(nullable: false),
                        DateUploaded = c.DateTime(nullable: false, storeType: "date"),
                        DateModified = c.DateTime(nullable: false, storeType: "date"),
                        PublicID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FileID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Projects", t => t.PublicID, cascadeDelete: true)
                .Index(t => t.PublicID)
                .Index(t => t.ApplicationUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "PublicID", "dbo.Projects");
            DropForeignKey("dbo.Files", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "ApplicationUserID" });
            DropIndex("dbo.Files", new[] { "PublicID" });
            DropTable("dbo.Files");
        }
    }
}
