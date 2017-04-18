using Project.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project.Controllers
{
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Student model)
        {
            if (ModelState.IsValid)
            {
                model.UserName = model.Email;
                var result = await SignUp.CreateAsync(model, model.Password);
                if (result.Succeeded)
                    return PartialView("Row", DB.Students.FirstOrDefault(s => s.Id == model.Id));
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
            else
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
                return Json(new { Success = true, Id = Id });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // END CRUD

        //exporting data to excel

        public ActionResult ExportToExcel()
        {
            var stud = DB.Students;
            //SelectList deplist = new SelectList(stud.ToList(), "depId", "name");

            var gv = new GridView();
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
            var depts = DB.Departments;
            SelectList deplist = new SelectList(depts.ToList(), "Id", "Name");
            ViewBag.deplist = deplist;
            return View();
        }

        [HttpPost]
        public ActionResult stuInDeps(int deptId)
        {
            var ListOfStuIn = DB.Students.Select(l => l.DepartmentId == deptId).ToList();
            var ListOfStuOut = DB.Students.Select(l => l.DepartmentId != deptId).ToList();
            ViewBag.ListOfStuIn = ListOfStuIn;
            ViewBag.ListOfStuOut = ListOfStuOut;
            return View();
        }
    }
}