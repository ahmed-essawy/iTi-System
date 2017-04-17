using Project.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class VacationController : MainController
    {
        // GET: Vacation
        // START CRUD
        public ActionResult Index()
        {
            return View(DB.Vacations);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Vacation model)
        {
            if (ModelState.IsValid)
            {
                DB.Vacations.Add(model);
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            else
                return PartialView("Row");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            return PartialView("Edit", DB.Vacations.FirstOrDefault(s => s.Id == Id));
        }

        [HttpPost]
        public ActionResult Edit(Vacation model)
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
        public ActionResult Delete(int Id)
        {
            DB.Vacations.Remove(DB.Vacations.FirstOrDefault(s => s.Id == Id));
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