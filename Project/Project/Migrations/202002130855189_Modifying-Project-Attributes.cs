namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyingProjectAttributes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.Projects", "ProjectName", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Projects", "ProjectType", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Projects", "ProjectDescription", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Projects", "ApplicationUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Projects", "ApplicationUser_Id");
            AddForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.Projects", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Projects", "ProjectDescription", c => c.String(maxLength: 250));
            AlterColumn("dbo.Projects", "ProjectType", c => c.String(maxLength: 50));
            AlterColumn("dbo.Projects", "ProjectName", c => c.String(maxLength: 25));
            CreateIndex("dbo.Projects", "ApplicationUser_Id");
            AddForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
