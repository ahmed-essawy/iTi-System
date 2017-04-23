using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project.Models;

namespace Project.Controllers
{
    public class PermissionController : MainController
    {
        // GET: Permission
        // START CRUD
        public ActionResult Index()
        {
            string InId = User.Identity.GetUserId();
            int DpId = DB.Departments.Where(a => a.ManagerId == InId).Select(a => a.Id).First();
            List<Permission> permission = DB.Permissions.Where(a => a.Instructor.DepartmentId == DpId).ToList();
            return View(permission);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.StList = new SelectList(DB.Students, "Id", "Name");
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Permission model)
        {
            ModelState.Remove("InstructorId");
            if (ModelState.IsValid)
            {
                model.InstructorId = User.Identity.GetUserId();
                model.Instructor = DB.Instructors.FirstOrDefault(i => i.Id == model.InstructorId);
                model.Student = DB.Students.FirstOrDefault(s => s.Id == model.StudentId);
                DB.Permissions.Add(model);
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            return PartialView("Row");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            return PartialView("Edit", DB.Permissions.FirstOrDefault(p => p.Id == Id));
        }

        [HttpPost]
        public ActionResult Edit(Permission model)
        {
            ModelState.Remove("InstructorId");
            if (ModelState.IsValid)
            {
                Permission perm = DB.Permissions.FirstOrDefault(p => p.Id == model.Id);
                perm.EndDate = model.EndDate;
                perm.Reason = model.Reason;
                DB.SaveChanges();
                return PartialView("Row", perm);
            }
            return PartialView("Row");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            DB.Permissions.Remove(DB.Permissions.FirstOrDefault(s => s.Id == Id));
            try
            {
                DB.SaveChanges();
                return Json(new {Success = true, Id});
            } catch (Exception ex) { return Json(new {Success = false, ex.Message}); }
        }

        // END CRUD
    }
}