namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Build15 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Dp_Id", "dbo.Departments");
            AddForeignKey("dbo.AspNetUsers", "Dp_Id", "dbo.Departments", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Dp_Id", "dbo.Departments");
            AddForeignKey("dbo.AspNetUsers", "Dp_Id", "dbo.Departments", "Id");
        }
    }
}
