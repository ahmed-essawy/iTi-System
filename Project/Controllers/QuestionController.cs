using System;
using System.Linq;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class QuestionController : MainController
    {
        // GET: Question
        public ActionResult Index()
        {
            return View(DB.Questions);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CrList = new SelectList(DB.Courses, "Id", "Name");
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Question model, string[] ans)
        {
            model.Course = DB.Courses.FirstOrDefault(c => c.Id == model.CourseId);
            if (ModelState.IsValid && (model.Header == QuestionType.TF || ans.Contains(model.CorrectAnswer)))
            {
                DB.Questions.Add(model);
                if (model.Header == QuestionType.MCQ && ans != null) foreach (string item in ans) DB.Answers.Add(new Answer { QuestionId = model.Id, Choice = item });
                DB.SaveChanges();
                return PartialView("Row", model);
            }
            return Json(new { Success = false, Message = "Data not valid!" });
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            DB.Answers.RemoveRange(DB.Answers.Where(a => a.QuestionId == Id));
            DB.Questions.Remove(DB.Questions.FirstOrDefault(q => q.Id == Id));
            try
            {
                DB.SaveChanges();
                return Json(new { Success = true, Id });
            }
            catch (Exception ex) { return Json(new { Success = false, ex.Message }); }
        }
    }
}