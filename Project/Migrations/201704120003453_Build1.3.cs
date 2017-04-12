namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Build13 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Attendances");
            AlterColumn("dbo.Attendances", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Attendances", "Arrival", c => c.DateTime());
            AlterColumn("dbo.Attendances", "Leaving", c => c.DateTime());
            AddPrimaryKey("dbo.Attendances", new[] { "Date", "St_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Attendances");
            AlterColumn("dbo.Attendances", "Leaving", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Attendances", "Arrival", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Attendances", "Date", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddPrimaryKey("dbo.Attendances", new[] { "Date", "St_Id" });
        }
    }
}
