namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectUsersAndRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectUsers",
                c => new
                    {
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                        PublicID = c.String(nullable: false),
                        Projects_PublicID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ApplicationUserID)
                .ForeignKey("dbo.Projects", t => t.Projects_PublicID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Projects_PublicID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ProjectRoles",
                c => new
                    {
                        RoleID = c.String(nullable: false, maxLength: 128),
                        PublicID = c.String(),
                        Projects_PublicID = c.Int(),
                        Role_Id = c.String(maxLength: 128),
                        ProjectUsers_ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RoleID)
                .ForeignKey("dbo.Projects", t => t.Projects_PublicID)
                .ForeignKey("dbo.AspNetRoles", t => t.Role_Id)
                .ForeignKey("dbo.ProjectUsers", t => t.ProjectUsers_ApplicationUserID)
                .Index(t => t.Projects_PublicID)
                .Index(t => t.Role_Id)
                .Index(t => t.ProjectUsers_ApplicationUserID);
            
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectUsers", "Projects_PublicID", "dbo.Projects");
            DropForeignKey("dbo.ProjectRoles", "ProjectUsers_ApplicationUserID", "dbo.ProjectUsers");
            DropForeignKey("dbo.ProjectRoles", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProjectRoles", "Projects_PublicID", "dbo.Projects");
            DropIndex("dbo.ProjectRoles", new[] { "ProjectUsers_ApplicationUserID" });
            DropIndex("dbo.ProjectRoles", new[] { "Role_Id" });
            DropIndex("dbo.ProjectRoles", new[] { "Projects_PublicID" });
            DropIndex("dbo.ProjectUsers", new[] { "User_Id" });
            DropIndex("dbo.ProjectUsers", new[] { "Projects_PublicID" });
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropTable("dbo.ProjectRoles");
            DropTable("dbo.ProjectUsers");
        }
    }
}
