namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTasks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTasks",
                c => new
                    {
                        TaskID = c.Int(nullable: false),
                        ProjectUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskID, t.ProjectUserID })
                .ForeignKey("dbo.ProjectUsers", t => t.ProjectUserID, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.TaskID, cascadeDelete: true)
                .Index(t => t.TaskID)
                .Index(t => t.ProjectUserID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskID = c.Int(nullable: false, identity: true),
                        PublicID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        TaskDescription = c.String(nullable: false),
                        IsComplete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TaskID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Projects", t => t.PublicID, cascadeDelete: true)
                .Index(t => t.PublicID)
                .Index(t => t.ApplicationUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTasks", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "PublicID", "dbo.Projects");
            DropForeignKey("dbo.Tasks", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserTasks", "ProjectUserID", "dbo.ProjectUsers");
            DropIndex("dbo.Tasks", new[] { "ApplicationUserID" });
            DropIndex("dbo.Tasks", new[] { "PublicID" });
            DropIndex("dbo.UserTasks", new[] { "ProjectUserID" });
            DropIndex("dbo.UserTasks", new[] { "TaskID" });
            DropTable("dbo.Tasks");
            DropTable("dbo.UserTasks");
        }
    }
}
