using System;
using Project.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Project.Controllers
{
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
            ViewBag.ManagersList = new SelectList(DB.Instructors, "Id", "Name");
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                model.Manager = DB.Instructors.FirstOrDefault(i => i.Id == model.ManagerId);
                DB.Departments.Add(model);
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            else
                return PartialView("Row");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ViewBag.ManagersList = new SelectList(DB.Instructors, "Id", "Name");
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
            else
                return PartialView("Row");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            foreach (Instructor item in DB.Instructors.Where(i => i.DepartmentId == Id))
                item.DepartmentId = null;
            foreach (Student item in DB.Students.Where(i => i.DepartmentId == Id))
                item.DepartmentId = null;
            DB.Departments.Remove(DB.Departments.FirstOrDefault(s => s.Id == Id));
            try
            {
                DB.SaveChanges();
                return Json(new { Success = true, Id = Id });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // END CRUD
    }
}