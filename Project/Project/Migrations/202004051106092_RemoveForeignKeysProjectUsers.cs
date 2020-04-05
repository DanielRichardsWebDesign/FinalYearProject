namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveForeignKeysProjectUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectRoles", "Projects_PublicID", "dbo.Projects");
            DropForeignKey("dbo.ProjectRoles", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProjectRoles", "ProjectUsers_ApplicationUserID", "dbo.ProjectUsers");
            DropForeignKey("dbo.ProjectUsers", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectUsers", new[] { "User_Id" });
            DropIndex("dbo.ProjectRoles", new[] { "Projects_PublicID" });
            DropIndex("dbo.ProjectRoles", new[] { "Role_Id" });
            DropIndex("dbo.ProjectRoles", new[] { "ProjectUsers_ApplicationUserID" });
            DropColumn("dbo.ProjectUsers", "User_Id");
            DropTable("dbo.ProjectRoles");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.RoleID);
            
            AddColumn("dbo.ProjectUsers", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.ProjectRoles", "ProjectUsers_ApplicationUserID");
            CreateIndex("dbo.ProjectRoles", "Role_Id");
            CreateIndex("dbo.ProjectRoles", "Projects_PublicID");
            CreateIndex("dbo.ProjectUsers", "User_Id");
            AddForeignKey("dbo.ProjectUsers", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ProjectRoles", "ProjectUsers_ApplicationUserID", "dbo.ProjectUsers", "ApplicationUserID");
            AddForeignKey("dbo.ProjectRoles", "Role_Id", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.ProjectRoles", "Projects_PublicID", "dbo.Projects", "PublicID");
        }
    }
}
