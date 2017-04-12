using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project.Models;

namespace Project.Controllers
{
    public class AttendanceController : Controllere
    {
        // GET: Attendance
        public ActionResult Index()
        {
            return View(DB.Attendaces);
        }

        public ActionResult Create()
        {
            foreach (Student std in DB.Students)
                if (DB.Attendaces.Where(d => d.StudentId == std.Id).Where(d => d.Date.Day == DateTime.Now.Day).Where(d => d.Date.Month == DateTime.Now.Month).Count(d => d.Date.Year == DateTime.Now.Year) <= 0)
                    DB.Attendaces.Add(new Attendance(std.Id));
            DB.SaveChanges();
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Display()
        {
            return RedirectToAction("Index");
        }

        public ActionResult Arrive(string Id)
        {
            DB.Attendaces.FirstOrDefault(d => d.StudentId == Id).ArrivalTime = DateTime.Now;
            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Leave(string Id)
        {
            DB.Attendaces.FirstOrDefault(d => d.StudentId == Id).LeavingTime = DateTime.Now;
            DB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}