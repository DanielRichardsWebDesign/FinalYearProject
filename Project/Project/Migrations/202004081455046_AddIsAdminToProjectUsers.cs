namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAdminToProjectUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectUsers", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectUsers", "IsAdmin");
        }
    }
}
