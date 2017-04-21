using System.Web.Mvc;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.HttpMethod == "POST") return PartialView();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            if (Request.HttpMethod == "POST") return PartialView();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            if (Request.HttpMethod == "POST") return PartialView();
            return View();
        }
    }
}