using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project.Models;

namespace Project.Controllers
{
    public class InstructorController : MainController
    {
        // GET: Instructor
        // START CRUD
        public ActionResult Index() => View(DB.Instructors);

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
                    if (quals != null) foreach (string item in quals) if (item != string.Empty) DB.Qualifications.Add(new Qualification {Name = item, InstructorId = model.Id});
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
                if (quals != null) foreach (string item in quals) if (item != string.Empty) DB.Qualifications.Add(new Qualification {Name = item, InstructorId = model.Id});
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
                return Json(new {Success = true, Id});
            } catch (Exception ex) { return Json(new {Success = false, ex.Message}); }
        }

        // END CRUD
    }
}