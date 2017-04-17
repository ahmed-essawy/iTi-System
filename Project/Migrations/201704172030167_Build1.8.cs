namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Build18 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Vacations", name: "StartDate", newName: "Start_Date");
            DropPrimaryKey("dbo.Permissions");
            AddColumn("dbo.Permissions", "Start_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Permissions", "End_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Vacations", "End_Date", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.Permissions", new[] { "St_Id", "Start_Date" });
            DropColumn("dbo.Permissions", "Date");
            DropColumn("dbo.Permissions", "Duration");
            DropColumn("dbo.Vacations", "Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vacations", "Duration", c => c.Int(nullable: false));
            AddColumn("dbo.Permissions", "Duration", c => c.Int(nullable: false));
            AddColumn("dbo.Permissions", "Date", c => c.DateTime(nullable: false));
            DropPrimaryKey("dbo.Permissions");
            DropColumn("dbo.Vacations", "End_Date");
            DropColumn("dbo.Permissions", "End_Date");
            DropColumn("dbo.Permissions", "Start_Date");
            AddPrimaryKey("dbo.Permissions", new[] { "St_Id", "Date" });
            RenameColumn(table: "dbo.Vacations", name: "Start_Date", newName: "StartDate");
        }
    }
}
