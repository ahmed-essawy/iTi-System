using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    [Authorize(Roles = "Instructor")]
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
            foreach (Student std in DB.Students) if (DB.Attendaces.Where(d => d.StudentId == std.Id).Where(d => d.Date.Day == DateTime.Now.Day).Where(d => d.Date.Month == DateTime.Now.Month).Count(d => d.Date.Year == DateTime.Now.Year) <= 0) atts.Add(new Attendance(std.Id));
            DB.Attendaces.AddRange(atts);
            DB.SaveChanges();
            if (Request.HttpMethod == "POST") return PartialView("IndexList", atts.OrderByDescending(a => a.Date));
            return Json(new { Success = true, records = atts.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Arrive(string Id)
        {
            DB.Attendaces.FirstOrDefault(d => d.StudentId == Id && d.Date.Day == DateTime.Now.Day && d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year).ArrivalTime = DateTime.Now;
            DB.DailyReports.FirstOrDefault(d => d.StudentId == Id && d.Date.Day == DateTime.Now.Day && d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year).ArrivalTime = DateTime.Now;
            DB.SaveChanges();
            Attendance att = DB.Attendaces.FirstOrDefault(d => d.StudentId == Id && d.Date.Day == DateTime.Now.Day && d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year);
            return PartialView("Row", att);
        }

        [HttpPost]
        public ActionResult Leave(string Id)
        {
            DB.Attendaces.FirstOrDefault(d => d.StudentId == Id && d.Date.Day == DateTime.Now.Day && d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year).LeavingTime = DateTime.Now;
            DB.DailyReports.FirstOrDefault(d => d.StudentId == Id && d.Date.Day == DateTime.Now.Day && d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year).LeavingTime = DateTime.Now;
            DB.SaveChanges();
            Attendance att = DB.Attendaces.FirstOrDefault(d => d.StudentId == Id && d.Date.Day == DateTime.Now.Day && d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year);
            return PartialView("Row", att);
        }

        // Should run using cronjobs when you need take attendance
        public ActionResult Attendance()
        {
            ApplicationDbContext DB = new ApplicationDbContext();
            bool isVacation = DB.Vacations.Any(v => DateTime.Today >= v.StartDate && DateTime.Today <= v.EndDate);
            if (!isVacation)
            {
                List<DailyReport> list = new List<DailyReport>();
                foreach (Student item in DB.Students) list.Add(IsLate(item.Id));
                if (Request.HttpMethod == "POST") return PartialView("IndexList", DB.Attendaces.OrderByDescending(a => a.Date));
                return Json(new { Success = true, Vacation = false, Students = list }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true, Vacation = true, Absent = 0, Present = 0 }, JsonRequestBehavior.AllowGet);
        }

        private DailyReport IsLate(string Id)
        {
            ApplicationDbContext DB = new ApplicationDbContext();
            int degrees = 0;
            Student std = DB.Students.FirstOrDefault(s => s.Id == Id);
            Create();
            Attendance att = DB.Attendaces.FirstOrDefault(a => a.StudentId == std.Id && a.Date.Day == DateTime.Now.Day && a.Date.Month == DateTime.Now.Month && a.Date.Year == DateTime.Now.Year);
            if (att != null)
            {
                if (att.ArrivalTime == null)
                {
                    bool havePermission = DB.Permissions.Any(p => p.StudentId == std.Id && DateTime.Now >= p.StartDate && DateTime.Now <= p.EndDate);
                    if (havePermission)
                    {
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
                    }
                    else degrees = 25;
                    ++std.Absences;
                    std.Degrees -= degrees;
                }
            }
            DailyReport todayReport = new DailyReport { StudentId = Id, Date = DateTime.Today, Absences = std.Absences, Degrees = degrees, ArrivalTime = att.ArrivalTime, LeavingTime = att.LeavingTime };
            if (!DB.DailyReports.Any(r => r.StudentId == todayReport.StudentId && r.Date == todayReport.Date))
            {
                DB.DailyReports.Add(todayReport);
                DB.Configuration.ValidateOnSaveEnabled = false;
                DB.SaveChanges();
                return todayReport;
            }
            return new DailyReport { StudentId = Id, Date = DateTime.Today, Absences = std.Absences, Degrees = 0, ArrivalTime = att.ArrivalTime, LeavingTime = att.LeavingTime };
        }

        [HttpGet]
        public ActionResult AttendanceSpecificDate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AttendanceSpecificDate(string date)
        {
            DateTime sDate = DateTime.Parse(date);
            if (!DB.Vacations.Any(v => sDate >= v.StartDate && sDate <= v.EndDate))
            {
                IEnumerable<DailyReport> repList = DB.DailyReports.Where(a => a.Date.Day == sDate.Day && a.Date.Month == sDate.Month && a.Date.Year == sDate.Year);
                return PartialView("DailyReport", repList);
            }
            return Content("<thead><tr><td>It was a vacation</td></tr><thead>");
        }

        [HttpGet]
        public ActionResult AttendancePeriodDate()
        {
            ViewBag.StList = new SelectList(DB.Students, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult AttendancePeriodDate(string studentId, string startDate, string endDate)
        {
            DateTime sDate = DateTime.Parse(startDate), eDate = DateTime.Parse(endDate);
            IEnumerable<DailyReport> repList = DB.DailyReports.Where(a => a.StudentId == studentId && a.Date >= sDate && a.Date <= eDate);
            return PartialView("DailyReport", repList);
        }

        [HttpGet]
        public ActionResult PermissionPeriodDate()
        {
            ViewBag.StList = new SelectList(DB.Students, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult PermissionPeriodDate(string studentId, string startDate, string endDate)
        {
            DateTime sDate = DateTime.Parse(startDate), eDate = DateTime.Parse(endDate);
            IEnumerable<Permission> permList = DB.Permissions.Where(a => a.StudentId == studentId && a.StartDate >= sDate && a.EndDate <= eDate);
            return PartialView("PermissionReport", permList);
        }
    }
}