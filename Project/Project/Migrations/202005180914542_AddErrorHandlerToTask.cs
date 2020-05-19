namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddErrorHandlerToTask : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tasks", "TaskDescription", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "TaskDescription", c => c.String(nullable: false));
        }
    }
}
