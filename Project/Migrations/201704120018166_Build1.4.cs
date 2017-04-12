namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Build14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Degrees", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Degrees");
        }
    }
}
