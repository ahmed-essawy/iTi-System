using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class DepartmentController : MainController
    {
        // GET: Department
        // START CRUD
        public ActionResult Index()
        {
            return View(DB.Departments);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ManagersList = new SelectList(DB.Instructors.Where(i => i.Status == Status.Internal), "Id", "Name");
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                model.Manager = DB.Instructors.FirstOrDefault(i => i.Id == model.ManagerId);
                DB.Instructors.FirstOrDefault(i => i.Id == model.ManagerId).DepartmentId = model.Id;
                DB.Departments.Add(model);
                DB.Configuration.ValidateOnSaveEnabled = false;
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            return PartialView("Row");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ViewBag.ManagersList = new SelectList(DB.Instructors.Where(i => i.Status == Status.Internal && i.DepartmentId == Id), "Id", "Name");
            return PartialView("Edit", DB.Departments.FirstOrDefault(s => s.Id == Id));
        }

        [HttpPost]
        public ActionResult Edit(Department model)
        {
            if (ModelState.IsValid)
            {
                model.Manager = DB.Instructors.FirstOrDefault(i => i.Id == model.ManagerId);
                DB.Entry(model).State = EntityState.Modified;
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            return PartialView("Row");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            foreach (Instructor item in DB.Instructors.Where(i => i.DepartmentId == Id)) item.DepartmentId = null;
            foreach (Student item in DB.Students.Where(i => i.DepartmentId == Id)) item.DepartmentId = null;
            DB.Departments.Remove(DB.Departments.FirstOrDefault(s => s.Id == Id));
            try
            {
                DB.SaveChanges();
                return Json(new { Success = true, Id });
            }
            catch (Exception ex) { return Json(new { Success = false, ex.Message }); }
        }

        // END CRUD
        [HttpGet]
        public ActionResult DepartmentStudents()
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult DepartmentStudents(int departmentId)
        {
            ViewBag.StList = DB.Students.Where(s => s.DepartmentId == departmentId);
            return PartialView("PartialDepartmentStudents", DB.Departments.FirstOrDefault(d => d.Id == departmentId));
        }

        [HttpGet]
        public ActionResult DepartmentInstructors()
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult DepartmentInstructors(int departmentId)
        {
            ViewBag.InList = DB.Instructors.Where(s => s.DepartmentId == departmentId);
            return PartialView("PartialDepartmentInstructors", DB.Departments.FirstOrDefault(d => d.Id == departmentId));
        }

        [HttpGet]
        public ActionResult DepartmentCourses()
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult DepartmentCourses(int departmentId)
        {
            ViewBag.CrList = DB.Courses.Where(c => c.Departments.Any(d => d.Id == departmentId));
            return PartialView("PartialDepartmentCourses", DB.Departments.FirstOrDefault(d => d.Id == departmentId));
        }

        [HttpGet]
        public ActionResult DepartmentManagerChange()
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult DepartmentManagerChange(int departmentId)
        {
            ViewBag.InList = DB.Instructors.Where(i => i.Status == Status.Internal && i.DepartmentId == departmentId);
            return PartialView("PartialDepartmentManagerChange", DB.Departments.FirstOrDefault(d => d.Id == departmentId));
        }

        [HttpPost]
        public ActionResult ChangeManager(int Id, string managerId)
        {
            DB.Departments.FirstOrDefault(d => d.Id == Id).ManagerId = managerId;
            DB.SaveChanges();
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}