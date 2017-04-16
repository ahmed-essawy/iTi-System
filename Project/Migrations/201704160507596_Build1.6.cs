namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Build16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Absences", c => c.Int(nullable: false));
            DropColumn("dbo.Students", "Attendance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "Attendance", c => c.Int(nullable: false));
            DropColumn("dbo.Students", "Absences");
        }
    }
}
