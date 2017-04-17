namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Build17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Permissions",
                    c => new
                    {
                        St_Id = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Id = c.Int(nullable: false, identity: true),
                        InstructorId = c.String(nullable: false, maxLength: 128),
                        Duration = c.Int(nullable: false),
                        Reason = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => new { t.St_Id, t.Date })
                .ForeignKey("dbo.Instructors", t => t.InstructorId)
                .ForeignKey("dbo.Students", t => t.St_Id)
                .Index(t => t.St_Id)
                .Index(t => t.Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "St_Id", "dbo.Students");
            DropForeignKey("dbo.Permissions", "InstructorId", "dbo.Instructors");
            DropIndex("dbo.Permissions", new[] { "Id" });
            DropIndex("dbo.Permissions", new[] { "St_Id" });
            DropTable("dbo.Permissions");
        }
    }
}