using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Project.Models;

namespace Project.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class StudentController : MainController
    {
        // GET: Student
        // START CRUD
        public ActionResult Index()
        {
            return View(DB.Students);
        }

        [HttpPost]
        public ActionResult Details(string Id)
        {
            return PartialView(DB.Students.FirstOrDefault(a => a.Id == Id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
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

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            ViewBag.DpList = new SelectList(DB.Departments, "Id", "Name");
            return PartialView("Edit", DB.Students.FirstOrDefault(s => s.Id == Id));
        }

        [HttpPost]
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

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            DB.Attendaces.RemoveRange(DB.Attendaces.Where(s => s.StudentId == Id));
            DB.Students.Remove(DB.Students.FirstOrDefault(s => s.Id == Id));
            try
            {
                DB.SaveChanges();
                return Json(new { Success = true, Id });
            }
            catch (Exception ex) { return Json(new { Success = false, ex.Message }); }
        }

        // END CRUD

        //exporting data to excel

        public ActionResult ExportToExcel()
        {
            DbSet<Student> stud = DB.Students;
            //SelectList deplist = new SelectList(stud.ToList(), "depId", "name");
            GridView gv = new GridView();
            gv.DataSource = stud;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "DBlication/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }

        public ActionResult stuInDeps()
        {
            DbSet<Department> depts = DB.Departments;
            SelectList deplist = new SelectList(depts.ToList(), "Id", "Name");
            ViewBag.deplist = deplist;
            return View();
        }

        [HttpPost]
        public ActionResult stuInDeps(int deptId)
        {
            List<bool> ListOfStuIn = DB.Students.Select(l => l.DepartmentId == deptId).ToList();
            List<bool> ListOfStuOut = DB.Students.Select(l => l.DepartmentId != deptId).ToList();
            ViewBag.ListOfStuIn = ListOfStuIn;
            ViewBag.ListOfStuOut = ListOfStuOut;
            return View();
        }
    }
}