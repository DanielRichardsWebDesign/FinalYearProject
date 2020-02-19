namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectDateColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "DateCreated", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "DateCreated");
        }
    }
}
