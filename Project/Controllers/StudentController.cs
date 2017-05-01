using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Office.Interop.Excel;
using Project.Models;

namespace Project.Controllers
{
    public class StudentController : MainController
    {
        // GET: Student
        // START CRUD
        [Authorize(Roles = "Instructor,Administrator")]
        public ActionResult Index()
        {
            return View(DB.Students);
        }

        [HttpPost, Authorize(Roles = "Instructor,Administrator")]
        public ActionResult Details(string Id)
        {
            return PartialView(DB.Students.FirstOrDefault(a => a.Id == Id));
        }

        [HttpGet, Authorize(Roles = "Instructor,Administrator")]
        public ActionResult Create()
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult> Create(Student model)
        {
            if (ModelState.IsValid)
            {
                model.UserName = model.Email;
                IdentityResult result = await SignUp.CreateAsync(model, model.Password);
                if (result.Succeeded) return PartialView("Row", DB.Students.FirstOrDefault(s => s.Id == model.Id));
                ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
                return PartialView(model);
            }
            return PartialView("Row");
        }

        [HttpGet, Authorize(Roles = "Instructor,Administrator")]
        public ActionResult Edit(string Id)
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return PartialView("Edit", DB.Students.FirstOrDefault(s => s.Id == Id));
        }

        [HttpPost, Authorize(Roles = "Instructor,Administrator")]
        public ActionResult Edit(Student model)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                model.UserName = model.Email;
                model.Department = DB.Departments.FirstOrDefault(d => d.Id == model.DepartmentId);
                DB.Entry(model).State = EntityState.Modified;
                DB.Configuration.ValidateOnSaveEnabled = false;
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            return PartialView("Row");
        }

        [HttpPost, Authorize(Roles = "Instructor,Administrator")]
        public ActionResult Delete(string Id)
        {
            DB.Attendaces.RemoveRange(DB.Attendaces.Where(s => s.StudentId == Id));
            DB.Students.Remove(DB.Students.FirstOrDefault(s => s.Id == Id));
            try
            {
                DB.SaveChanges();
                return Json(new {Success = true, Id});
            } catch (Exception ex) { return Json(new {Success = false, ex.Message}); }
        }

        // END CRUD

        [HttpGet, Authorize(Roles = "Instructor,Administrator")]
        public ActionResult Import()
        {
            return PartialView();
        }

        [HttpPost, Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult> Import(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                ViewBag.error = "choose excel File";
                return PartialView();
            }
            if (excelFile.FileName.EndsWith(".xls") || excelFile.FileName.EndsWith(".xlsx"))
            {
                int count = 0;
                string path = Server.MapPath("~/Content/") + excelFile.FileName;
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                excelFile.SaveAs(path);
                Application application = new Application();
                Workbook workbook = application.Workbooks.Open(path);
                Worksheet worksheet = workbook.ActiveSheet;
                Range range = worksheet.Rows.CurrentRegion.EntireRow;
                for (int i = 1; i < range.Rows.Count + 1; i++)
                {
                    Student stud = new Student();
                    stud.FirstName = ((Range)range.Cells[i, 1]).Text;
                    stud.LastName = ((Range)range.Cells[i, 2]).Text;
                    string m = ((Range)range.Cells[i, 3]).Text;
                    stud.IsMarried = Convert.ToBoolean(m);
                    stud.Birthdate = DateTime.Parse(((Range)range.Cells[i, 4]).Text);
                    stud.PhoneNumber = ((Range)range.Cells[i, 5]).Text;
                    stud.UserName = stud.Email = ((Range)range.Cells[i, 6]).Text;
                    string x = ((Range)range.Cells[i, 8]).Text;
                    if (x.Length < 1) stud.DepartmentId = null;
                    else stud.DepartmentId = int.Parse(x);
                    stud.Password = stud.ConfirmPassword = ((Range)range.Cells[i, 7]).Text;
                    IdentityResult result = await SignUp.CreateAsync(stud, stud.Password);
                    if (result.Succeeded) ++count;
                }
                return Json(new {Success = true, Count = count});
            }
            return Json(new {Success = false});
        }

        [Authorize(Roles = "Instructor,Administrator")]
        public ActionResult stuInDeps()
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return View();
        }

        [HttpPost, Authorize(Roles = "Instructor,Administrator")]
        public ActionResult stuInDeps(int Department)
        {
            ViewBag.deptId = Department;
            // ViewBag.ListOfStuIn = DB.Students.Where(s => s.DepartmentId == Department).ToList();
            ViewBag.ListOfStuOut = DB.Students.Where(s => s.DepartmentId != Department).ToList();
            return PartialView("stuPartialView");
        }

        [HttpPost, Authorize(Roles = "Instructor,Administrator")]
        public ActionResult addStuToDep(string[] ListOfStuOut, int deptId)
        {
            Student st = new Student();
            Department de = new Department();
            de = DB.Departments.FirstOrDefault(r => r.Id == deptId);
            if (ListOfStuOut != null)
            {
                foreach (string z in ListOfStuOut)
                {
                    st = DB.Students.FirstOrDefault(f => f.Id == z);
                    st.DepartmentId = deptId;
                    DB.Entry(st).State = EntityState.Modified;
                    DB.SaveChanges();
                }
            }
            return RedirectToAction("stuInDeps");
        }

        ///
        //[HttpGet, Authorize(Roles = "Student")]
        //public ActionResult stuCrs()
        //{
        //    string id = User.Identity.GetUserId();
        //    Student std = new Student();
        //    std = DB.Students.FirstOrDefault(p => p.Id == id);
        //    //SelectList stuCrs = new SelectList(stu.ToList(), "Id", "Name");
        //    //ViewBag.stuCrs = stuCrs;
        //    ViewBag.std = std;
        //    return View();
        //}
        [HttpGet, Authorize(Roles = "Student")]
        public ActionResult stuCrs() // id of stu
        {
            string id = User.Identity.GetUserId();
            List<InstructorStudentCourse> ISC = new List<InstructorStudentCourse>();
            ISC = DB.InstructorStudentCourse.Where(u => u.StudentId == id).ToList();
            List<CourseGrade> CrsGrades = DB.InstructorStudentCourse.Where(isc => isc.StudentId == id).Select(c => new CourseGrade {CourseName = c.Course.Name, ExamGrade = c.ExamGrade, LabGrade = c.LabGrade}).ToList();
            return View(CrsGrades);
        }

        /// add evaluation
        [HttpGet, Authorize(Roles = "Student")]
        public ActionResult AddEval()
        {
            if (User.IsInRole("Student"))
            {
                string id = User.Identity.GetUserId();
                IQueryable<Course> crList = DB.InstructorStudentCourse.Where(p => p.StudentId == id && p.ExamGrade == null).Select(c => c.Course);
                ViewBag.CrList = new SelectList(crList, "Id", "Name");
                return View();
            }
            return PartialView("Error");
        }

        [HttpPost, Authorize(Roles = "Student")]
        public ActionResult AddEval(string crId)
        {
            ViewBag.InList = DB.InstructorStudentCourse.Where(g => g.CourseId == crId).Select(c => c.Instructor).Distinct();
            return PartialView("AddEvalPartial");
        }

        //Evaluation
        [HttpPost, Authorize(Roles = "Student")]
        public ActionResult SubmitEval(string[] InList, string[] EvList)
        {
            if (InList.Length == EvList.Length)
            {
                for (int i = 0; i < InList.Length; i++)
                {
                    string id = InList[i];
                    DB.InstructorStudentCourse.FirstOrDefault(s => s.InstructorId == id).InstructorEvaluation = int.Parse(EvList[i]);
                }
            }
            return Json(new {Success = true});
        }
    }
}