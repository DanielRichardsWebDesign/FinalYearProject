namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContainerNameToProjects : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectContainerName", c => c.String(nullable: false, maxLength: 63));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ProjectContainerName");
        }
    }
}
