namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Build11 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Departments", new[] { "Manager_Id" });
            AlterColumn("dbo.Departments", "Manager_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Departments", "Manager_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Departments", new[] { "Manager_Id" });
            AlterColumn("dbo.Departments", "Manager_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Departments", "Manager_Id");
        }
    }
}
