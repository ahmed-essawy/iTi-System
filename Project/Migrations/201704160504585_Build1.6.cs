namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Build16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Attendance", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Attendance");
        }
    }
}
