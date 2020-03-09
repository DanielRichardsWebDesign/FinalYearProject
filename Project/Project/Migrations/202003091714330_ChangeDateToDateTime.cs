namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDateToDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Files", "DateUploaded", c => c.DateTime(nullable: false, storeType: "datetime"));
            AlterColumn("dbo.Files", "DateModified", c => c.DateTime(nullable: false, storeType: "datetime"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Files", "DateModified", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Files", "DateUploaded", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
