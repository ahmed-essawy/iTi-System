using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project.Models;

namespace Project.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorController : MainController
    {
        // GET: Instructor
        // START CRUD
        public ActionResult Index()
        {
            return View(DB.Instructors);
        }

        [HttpPost]
        public ActionResult Details(string Id)
        {
            ViewBag.QuList = DB.Qualifications.Where(q => q.InstructorId == Id);
            return PartialView(DB.Instructors.FirstOrDefault(a => a.Id == Id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Instructor model, string[] quals)
        {
            if (ModelState.IsValid)
            {
                model.UserName = model.Email;
                if (model.Status == Status.External) model.DepartmentId = null;
                IdentityResult result = await SignUp.CreateAsync(model, model.Password);
                if (result.Succeeded)
                {
                    if (quals != null) foreach (string item in quals) if (item != string.Empty) DB.Qualifications.Add(new Qualification { Name = item, InstructorId = model.Id });
                    DB.SaveChanges();
                    return PartialView("Row", DB.Instructors.FirstOrDefault(i => i.Id == model.Id));
                }
                ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
                return PartialView(model);
            }
            return PartialView("Row");
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            ViewBag.QuList = DB.Qualifications.Where(q => q.InstructorId == Id);
            return PartialView("Edit", DB.Instructors.FirstOrDefault(s => s.Id == Id));
        }

        [HttpPost]
        public ActionResult Edit(Instructor model, string[] quals)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                DB.Qualifications.RemoveRange(DB.Qualifications.Where(q => q.InstructorId == model.Id));
                if (quals != null) foreach (string item in quals) if (item != string.Empty) DB.Qualifications.Add(new Qualification { Name = item, InstructorId = model.Id });
                model.UserName = model.Email;
                if (model.Status == Status.External) model.DepartmentId = null;
                model.Department = DB.Departments.FirstOrDefault(d => d.Id == model.DepartmentId);
                DB.Entry(model).State = EntityState.Modified;
                DB.Configuration.ValidateOnSaveEnabled = false;
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            return PartialView("Row");
        }

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            DB.Qualifications.RemoveRange(DB.Qualifications.Where(q => q.InstructorId == Id));
            DB.Instructors.Remove(DB.Instructors.FirstOrDefault(i => i.Id == Id));
            try
            {
                DB.SaveChanges();
                return Json(new { Success = true, Id });
            }
            catch (Exception ex) { return Json(new { Success = false, ex.Message }); }
        }

        // END CRUD
        [HttpGet]
        public ActionResult InstructorCourses()
        {
            ViewBag.InList = new SelectList(DB.Instructors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult InstructorCourses(string id)
        {
            List<Course> cr = DB.InstructorStudentCourse.Where(a => a.InstructorId == id).Select(s => s.Course).ToList();
            return PartialView("InsCrPartial", cr);
        }

        [HttpGet]
        public ActionResult AddDegree()
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return View();
        }

        public ActionResult Department(int Department)
        {
            ViewBag.CrList = new SelectList(DB.Departments.FirstOrDefault(a => a.Id == Department).Courses, "Id", "Name");
            return PartialView("CourseDropdownlist");
        }

        [HttpPost]
        public ActionResult AddDegree(int Department, string Course)
        {
            string InId = User.Identity.GetUserId();
            List<InstructorStudentCourse> st = DB.InstructorStudentCourse.Where(a => a.InstructorId == InId && a.CourseId == Course && a.Student.DepartmentId == Department).ToList();
            return PartialView("AddDegreePartial", st);
        }

        [HttpPost]
        public ActionResult SaveDegree(string[] StudentId, int[] Degree, string Course, int Department)
        {
            string InId = User.Identity.GetUserId();
            List<InstructorStudentCourse> st = DB.InstructorStudentCourse.Where(a => a.InstructorId == InId && a.CourseId == Course && a.Student.DepartmentId == Department).ToList();
            for (int i = 0; i < st.Count; i++) st[i].ExamGrade = Degree[i];
            DB.SaveChanges();
            return Json(true);
        }

        [HttpGet]
        public ActionResult Manager()
        {
            string InId = User.Identity.GetUserId();
            IQueryable<InstructorStudentCourse> stds = DB.InstructorStudentCourse.Where(i => i.Student.Department.ManagerId == InId);
            return View(stds);
        }

        [HttpGet]
        public ActionResult Evaluation()
        {
            string InId = User.Identity.GetUserId();
            int? DpId = DB.Departments.FirstOrDefault(a => a.ManagerId == InId)?.Id;
            if (DpId != null) return PartialView(DB.InstructorStudentCourse.Where(a => a.Instructor.DepartmentId == DpId));
            return PartialView("Error");
        }
    }
}