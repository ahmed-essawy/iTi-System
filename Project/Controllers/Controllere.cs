using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Project.Models;

namespace Project.Controllers
{
    public class Controllere : Controller
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
    }
}