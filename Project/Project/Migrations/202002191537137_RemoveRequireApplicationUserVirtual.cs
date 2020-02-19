namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequireApplicationUserVirtual : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ApplicationUserID" });
            AlterColumn("dbo.Projects", "ApplicationUserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Projects", "ApplicationUserID");
            AddForeignKey("dbo.Projects", "ApplicationUserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ApplicationUserID" });
            AlterColumn("dbo.Projects", "ApplicationUserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Projects", "ApplicationUserID");
            AddForeignKey("dbo.Projects", "ApplicationUserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
