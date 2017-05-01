using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
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

        [HttpGet]
        public ActionResult UploadExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                ViewBag.error = "Please Insert File";
                return View();
            }
            if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
            {
                string fileLocation = Server.MapPath("~/Content/") + file.FileName;
                if (System.IO.File.Exists(fileLocation)) System.IO.File.Delete(fileLocation);
                file.SaveAs(fileLocation);
                Application app = new Application();
                Workbook wb = app.Workbooks.Open(fileLocation);
                Worksheet ws = wb.ActiveSheet;
                Range rng = ws.Rows.CurrentRegion.EntireRow;
                for (int i = 2; i < rng.Count + 1; i++)
                {
                    string mng_id = ((Range)rng.Cells[i, 2]).Text;
                    if (mng_id.Length > 0)
                    {
                        if (DB.Instructors.Any(d => d.Id == mng_id))
                        {
                            Department dp = new Department { Name = ((Range)rng.Cells[i, 1]).Text, ManagerId = ((Range)rng.Cells[i, 2]).Text, Capacity = int.Parse(((Range)rng.Cells[i, 3]).Text) };
                            if (!DB.Departments.Any(d => d.Name == dp.Name && d.ManagerId == dp.ManagerId))
                            {
                                DB.Departments.Add(dp);
                                DB.SaveChanges();
                            }
                        }
                    }
                }
                wb.Close(true);
                app.Quit();
                return View("Index", DB.Departments);
            }
            ViewBag.error = "File must be Excel";
            return View();
        }
    }
}