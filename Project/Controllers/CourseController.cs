using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class CourseController : MainController
    {
        // GET: Course
        // START CRUD
        public ActionResult Index()
        {
            return View(DB.Courses);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Course model)
        {
            if (ModelState.IsValid)
            {
                DB.Courses.Add(model);
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            else
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
            else
                return PartialView("Row");
        }

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            DB.Courses.Remove(DB.Courses.FirstOrDefault(s => s.Id == Id));
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