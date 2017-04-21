using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class CourseController : MainController
    {
        // GET: Course
        // START CRUD
        public ActionResult Index() => View(DB.Courses);

        [HttpGet]
        public ActionResult Create() => PartialView();

        [HttpPost]
        public ActionResult Create(Course model)
        {
            if (ModelState.IsValid)
            {
                DB.Courses.Add(model);
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            return PartialView("Row");
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            return PartialView("Edit", DB.Courses.FirstOrDefault(s => s.Id == Id));
        }

        [HttpPost]
        public ActionResult Edit(Course model)
        {
            if (ModelState.IsValid)
            {
                DB.Entry(model).State = EntityState.Modified;
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            return PartialView("Row");
        }

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            DB.Courses.Remove(DB.Courses.FirstOrDefault(s => s.Id == Id));
            try
            {
                DB.SaveChanges();
                return Json(new {Success = true, Id});
            } catch (Exception ex) { return Json(new {Success = false, ex.Message}); }
        }

        // END CRUD
    }
}