using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class AttendanceController : MainController
    {
        // GET: Attendance
        public ActionResult Index()
        {
            return View(DB.Attendaces.OrderByDescending(a => a.Date));
        }

        // Should run using cronjobs every day at 12:00 AM
        public ActionResult Create()
        {
            ApplicationDbContext DB = new ApplicationDbContext();
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
            DB.Attendaces.FirstOrDefault(d => d.StudentId == Id && d.Date.Day == DateTime.Now.Day && d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year).ArrivalTime = DateTime.Now;
            DB.SaveChanges();
            Attendance att = DB.Attendaces.FirstOrDefault(d => d.StudentId == Id && d.Date.Day == DateTime.Now.Day && d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year);
            return PartialView("Row", att);
        }

        [HttpPost]
        public ActionResult Leave(string Id)
        {
            DB.Attendaces.FirstOrDefault(d => d.StudentId == Id && d.Date.Day == DateTime.Now.Day &&
                                              d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year)
                .LeavingTime = DateTime.Now;
            DB.SaveChanges();
            Attendance att = DB.Attendaces.FirstOrDefault(
                d => d.StudentId == Id && d.Date.Day == DateTime.Now.Day && d.Date.Month == DateTime.Now.Month &&
                     d.Date.Year == DateTime.Now.Year);
            return PartialView("Row", att);
        }

        // Should run using cronjobs when you need take attendance
        public ActionResult Attendance()
        {
            ApplicationDbContext DB = new ApplicationDbContext();
            bool isVacation = DB.Vacations.Any(v => DateTime.Today >= v.StartDate && DateTime.Today < v.EndDate);
            if (!isVacation)
            {
                List<LateReport> list = new List<LateReport>();
                foreach (Student item in DB.Students)
                    list.Add(IsLate(item.Id));
                if (Request.HttpMethod == "POST")
                    return PartialView("IndexList", DB.Attendaces.OrderByDescending(a => a.Date));
                else
                    return Json(new { Success = true, Vacation = false, Students = list }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = true, Vacation = true, Absent = 0, Present = 0 }, JsonRequestBehavior.AllowGet);
        }

        private LateReport IsLate(string Id)
        {
            ApplicationDbContext DB = new ApplicationDbContext();
            int degrees = 0;
            Student std = DB.Students.FirstOrDefault(s => s.Id == Id);
            Create();
            Attendance att = DB.Attendaces.FirstOrDefault(a => a.StudentId == std.Id && a.Date.Day == DateTime.Now.Day && a.Date.Month == DateTime.Now.Month && a.Date.Year == DateTime.Now.Year);
            if (att != null)
                if (att.ArrivalTime == null)
                {
                    bool havePermission = DB.Permissions.Any(p => p.StudentId == std.Id && DateTime.Now >= p.StartDate && DateTime.Now < p.EndDate);
                    if (havePermission)
                        switch (std.Absences)
                        {
                            case 1:
                            case 2:
                            case 3:
                                degrees = 5;
                                break;

                            case 4:
                            case 5:
                            case 6:
                                degrees = 10;
                                break;

                            default:
                                degrees = 25;
                                break;
                        }
                    else
                        degrees = 25;
                    ++std.Absences;
                    std.Degrees -= degrees;
                    DB.Configuration.ValidateOnSaveEnabled = false;
                    DB.SaveChanges();
                }
            return new LateReport() { Id = Id, DateTime = DateTime.Now, Absences = std.Absences, Degrees = degrees };
        }

        [HttpGet]
        public ActionResult ReportSpecificDate()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ReportSpecificDate(string date)
        {
            DateTime sDate = DateTime.Parse(date);
            IEnumerable<Attendance> attList = DB.Attendaces.Where(a => a.Date.Day == sDate.Day && a.Date.Month == sDate.Month && a.Date.Year == sDate.Year);

            foreach (Attendance item in attList)
            {
                DB.Students.FirstOrDefault(s => s.Id == item.StudentId);
            }
            return PartialView();
        }
    }

    public struct LateReport
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public int Absences { get; set; }
        public int Degrees { get; set; }
    }
}