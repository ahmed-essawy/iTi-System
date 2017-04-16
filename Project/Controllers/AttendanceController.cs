using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project.Models;

namespace Project.Controllers
{
    public class AttendanceController : MainController
    {
        // GET: Attendance
        public ActionResult Index()
        {
            return View(DB.Attendaces.OrderByDescending(a => a.Date));
        }

        public ActionResult Create()
        {
            List<Attendance> atts = new List<Attendance>();
            foreach (Student std in DB.Students)
                if (DB.Attendaces.Where(d => d.StudentId == std.Id).Where(d => d.Date.Day == DateTime.Now.Day).Where(d => d.Date.Month == DateTime.Now.Month).Count(d => d.Date.Year == DateTime.Now.Year) <= 0)
                    atts.Add(new Attendance(std.Id));
            DB.Attendaces.AddRange(atts);
            DB.SaveChanges();
            if (Request.HttpMethod == "POST")
                return PartialView("IndexList", atts.OrderByDescending(a => a.Date));
            else
                return Json(new { Success = true, records = atts.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Arrive(string Id)
        {
            DB.Attendaces.Where(d => d.StudentId == Id).Where(d => d.Date.Day == DateTime.Now.Day).Where(d => d.Date.Month == DateTime.Now.Month).FirstOrDefault(d => d.Date.Year == DateTime.Now.Year).ArrivalTime = DateTime.Now;
            DB.SaveChanges();
            Attendance att = DB.Attendaces.Where(d => d.StudentId == Id).Where(d => d.Date.Day == DateTime.Now.Day).Where(d => d.Date.Month == DateTime.Now.Month).FirstOrDefault(d => d.Date.Year == DateTime.Now.Year);
            return PartialView("Row", att);
        }

        [HttpPost]
        public ActionResult Leave(string Id)
        {
            DB.Attendaces.Where(d => d.StudentId == Id).Where(d => d.Date.Day == DateTime.Now.Day).Where(d => d.Date.Month == DateTime.Now.Month).FirstOrDefault(d => d.Date.Year == DateTime.Now.Year).LeavingTime = DateTime.Now;
            DB.SaveChanges();
            Attendance att = DB.Attendaces.Where(d => d.StudentId == Id).Where(d => d.Date.Day == DateTime.Now.Day).Where(d => d.Date.Month == DateTime.Now.Month).FirstOrDefault(d => d.Date.Year == DateTime.Now.Year);
            return PartialView("Row", att);
        }
    }
}