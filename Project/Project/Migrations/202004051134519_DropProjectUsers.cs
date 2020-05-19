namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropProjectUsers : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ProjectUsers");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectUsers", "PublicID", "dbo.Projects");
            DropForeignKey("dbo.ProjectUsers", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectUsers", new[] { "PublicID" });
            DropIndex("dbo.ProjectUsers", new[] { "ApplicationUserID" });
            DropPrimaryKey("dbo.ProjectUsers");
            AlterColumn("dbo.ProjectUsers", "PublicID", c => c.Int());
            AlterColumn("dbo.ProjectUsers", "PublicID", c => c.String(nullable: false));
            AlterColumn("dbo.ProjectUsers", "ApplicationUserID", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.ProjectUsers", "ProjectUserID");
            AddPrimaryKey("dbo.ProjectUsers", "ApplicationUserID");
            RenameColumn(table: "dbo.ProjectUsers", name: "PublicID", newName: "Projects_PublicID");
            AddColumn("dbo.ProjectUsers", "PublicID", c => c.String(nullable: false));
            CreateIndex("dbo.ProjectUsers", "Projects_PublicID");
            AddForeignKey("dbo.ProjectUsers", "Projects_PublicID", "dbo.Projects", "PublicID");
        }
    }
}
