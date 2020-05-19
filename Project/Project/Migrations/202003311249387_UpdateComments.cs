namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComments : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "Comment", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "Comment", c => c.String(nullable: false));
        }
    }
}
