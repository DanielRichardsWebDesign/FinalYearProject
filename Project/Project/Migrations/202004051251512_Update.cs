namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectUsers", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectUsers", new[] { "ApplicationUserID" });
            AlterColumn("dbo.ProjectUsers", "ApplicationUserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.ProjectUsers", "ApplicationUserID");
            AddForeignKey("dbo.ProjectUsers", "ApplicationUserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectUsers", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectUsers", new[] { "ApplicationUserID" });
            AlterColumn("dbo.ProjectUsers", "ApplicationUserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.ProjectUsers", "ApplicationUserID");
            AddForeignKey("dbo.ProjectUsers", "ApplicationUserID", "dbo.AspNetUsers", "Id");
        }
    }
}
