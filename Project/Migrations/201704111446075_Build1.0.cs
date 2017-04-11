namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Build10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Choices",
                c => new
                    {
                        Qs_Id = c.Int(nullable: false),
                        Choice = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Qs_Id, t.Choice })
                .ForeignKey("dbo.Questions", t => t.Qs_Id, cascadeDelete: true)
                .Index(t => t.Qs_Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(nullable: false, maxLength: 200),
                        Body = c.String(nullable: false, maxLength: 500),
                        Correct = c.String(nullable: false, maxLength: 500),
                        Cr_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Cr_Id)
                .Index(t => t.Cr_Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        Lec_Duration = c.Int(nullable: false),
                        Lab_Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Capacity = c.Int(nullable: false),
                        Manager_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instructors", t => t.Manager_Id)
                .Index(t => t.Manager_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Fname = c.String(maxLength: 25),
                        Lname = c.String(maxLength: 25),
                        Bdate = c.DateTime(),
                        IsMarried = c.Boolean(),
                        Dp_Id = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Dp_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Dp_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Qualifications",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        In_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Name, t.In_Id })
                .ForeignKey("dbo.Instructors", t => t.In_Id)
                .Index(t => t.In_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Duration = c.Single(nullable: false),
                        From = c.Int(nullable: false),
                        Cr_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Cr_Id)
                .Index(t => t.Cr_Id);
            
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        St_Id = c.String(nullable: false, maxLength: 128),
                        Arrival = c.DateTime(),
                        Leaving = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Date, t.St_Id })
                .ForeignKey("dbo.Students", t => t.St_Id)
                .Index(t => t.St_Id);
            
            CreateTable(
                "dbo.In_St_Cr",
                c => new
                    {
                        InstructorId = c.String(nullable: false, maxLength: 128),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        CourseId = c.String(nullable: false, maxLength: 128),
                        Eval = c.Int(),
                    })
                .PrimaryKey(t => new { t.InstructorId, t.StudentId, t.CourseId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Instructors", t => t.InstructorId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.InstructorId)
                .Index(t => t.StudentId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.St_Ex",
                c => new
                    {
                        St_Id = c.String(nullable: false, maxLength: 128),
                        Ex_Id = c.Int(nullable: false),
                        Grade = c.Single(),
                    })
                .PrimaryKey(t => new { t.St_Id, t.Ex_Id })
                .ForeignKey("dbo.Exams", t => t.Ex_Id, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.St_Id)
                .Index(t => t.St_Id)
                .Index(t => t.Ex_Id);
            
            CreateTable(
                "dbo.Vacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        StartDate = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepartmentCourses",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        Course_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.Course_Id })
                .ForeignKey("dbo.Departments", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "dbo.ExamQuestions",
                c => new
                    {
                        Exam_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Exam_Id, t.Question_Id })
                .ForeignKey("dbo.Exams", t => t.Exam_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Exam_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.Instructors",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Grad_Year = c.Int(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Instructors", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Dp_Id", "dbo.Departments");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.St_Ex", "St_Id", "dbo.Students");
            DropForeignKey("dbo.St_Ex", "Ex_Id", "dbo.Exams");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.In_St_Cr", "StudentId", "dbo.Students");
            DropForeignKey("dbo.In_St_Cr", "InstructorId", "dbo.Instructors");
            DropForeignKey("dbo.In_St_Cr", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Attendances", "St_Id", "dbo.Students");
            DropForeignKey("dbo.Choices", "Qs_Id", "dbo.Questions");
            DropForeignKey("dbo.Questions", "Cr_Id", "dbo.Courses");
            DropForeignKey("dbo.ExamQuestions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.ExamQuestions", "Exam_Id", "dbo.Exams");
            DropForeignKey("dbo.Exams", "Cr_Id", "dbo.Courses");
            DropForeignKey("dbo.Departments", "Manager_Id", "dbo.Instructors");
            DropForeignKey("dbo.Qualifications", "In_Id", "dbo.Instructors");
            DropForeignKey("dbo.DepartmentCourses", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.DepartmentCourses", "Department_Id", "dbo.Departments");
            DropIndex("dbo.Students", new[] { "Id" });
            DropIndex("dbo.Instructors", new[] { "Id" });
            DropIndex("dbo.ExamQuestions", new[] { "Question_Id" });
            DropIndex("dbo.ExamQuestions", new[] { "Exam_Id" });
            DropIndex("dbo.DepartmentCourses", new[] { "Course_Id" });
            DropIndex("dbo.DepartmentCourses", new[] { "Department_Id" });
            DropIndex("dbo.St_Ex", new[] { "Ex_Id" });
            DropIndex("dbo.St_Ex", new[] { "St_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.In_St_Cr", new[] { "CourseId" });
            DropIndex("dbo.In_St_Cr", new[] { "StudentId" });
            DropIndex("dbo.In_St_Cr", new[] { "InstructorId" });
            DropIndex("dbo.Attendances", new[] { "St_Id" });
            DropIndex("dbo.Exams", new[] { "Cr_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Qualifications", new[] { "In_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Dp_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Departments", new[] { "Manager_Id" });
            DropIndex("dbo.Questions", new[] { "Cr_Id" });
            DropIndex("dbo.Choices", new[] { "Qs_Id" });
            DropTable("dbo.Students");
            DropTable("dbo.Instructors");
            DropTable("dbo.ExamQuestions");
            DropTable("dbo.DepartmentCourses");
            DropTable("dbo.Vacations");
            DropTable("dbo.St_Ex");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.In_St_Cr");
            DropTable("dbo.Attendances");
            DropTable("dbo.Exams");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Qualifications");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Departments");
            DropTable("dbo.Courses");
            DropTable("dbo.Questions");
            DropTable("dbo.Choices");
        }
    }
}
