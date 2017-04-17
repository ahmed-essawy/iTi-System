using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
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
            return PartialView(DB.Instructors.FirstOrDefault(a => a.Id == Id));
        }

        public ActionResult Qualifications(string Id)
        {
            return PartialView(DB.Qualifications.Where(q => q.InstructorId == Id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Instructor model, string[] quals)
        {
            if (ModelState.IsValid)
            {
                if (model.Status == Status.External) model.DepartmentId = null;
                var result = await SignUp.CreateAsync(model, model.Password);
                if (result.Succeeded)
                {
                    if (quals != null)
                        foreach (string item in quals)
                            if (item != String.Empty)
                                DB.Qualifications.Add(new Qualification { Name = item, InstructorId = model.Id });
                    DB.SaveChanges();
                    return PartialView("Row", DB.Instructors.FirstOrDefault(s => s.Id == model.Id));
                }
                else
                {
                    ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
                    return PartialView(model);
                }
            }
            else
                return PartialView("Row");
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return PartialView("Edit", DB.Instructors.FirstOrDefault(s => s.Id == Id));
        }

        [HttpPost]
        public ActionResult Edit(Instructor model, string[] quals)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                DB.Qualifications.RemoveRange(DB.Qualifications.Where(q => q.InstructorId == model.Id));
                if (quals != null)
                    foreach (string item in quals)
                        if (item != String.Empty)
                            DB.Qualifications.Add(new Qualification { Name = item, InstructorId = model.Id });
                if (model.Status == Status.External) model.DepartmentId = null;
                model.Department = DB.Departments.FirstOrDefault(d => d.Id == model.DepartmentId);
                DB.Entry(model).State = EntityState.Modified;
                DB.Configuration.ValidateOnSaveEnabled = false;
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            else
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