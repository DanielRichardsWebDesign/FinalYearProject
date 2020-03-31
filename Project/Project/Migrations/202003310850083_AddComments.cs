namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        Comment = c.String(nullable: false),
                        DateCommented = c.DateTime(nullable: false),
                        FileID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Files", t => t.FileID, cascadeDelete: true)
                .Index(t => t.FileID)
                .Index(t => t.ApplicationUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "FileID", "dbo.Files");
            DropForeignKey("dbo.Comments", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "ApplicationUserID" });
            DropIndex("dbo.Comments", new[] { "FileID" });
            DropTable("dbo.Comments");
        }
    }
}
