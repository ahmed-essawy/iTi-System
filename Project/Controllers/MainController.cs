using Microsoft.AspNet.Identity.Owin;
using Project.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class MainController : Controller
    {
        public ApplicationDbContext DB = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignIn
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager SignUp
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [HttpPost]
        public JsonResult IsUserNameExist(string UserName, string Id)
        {
            int count = DB.Instructors.Where(i => i.Id != Id).Count(i => i.UserName == UserName) +
                        DB.Students.Where(s => s.Id != Id).Count(s => s.UserName == UserName);
            return Json(count == 0);
        }

        [HttpPost]
        public ActionResult IsEmailExist(string Email, string Id)
        {
            int count = DB.Instructors.Where(i => i.Id != Id).Count(i => i.Email == Email) +
                        DB.Students.Where(s => s.Id != Id).Count(s => s.Email == Email);
            return Json(count == 0);
        }
    }
}