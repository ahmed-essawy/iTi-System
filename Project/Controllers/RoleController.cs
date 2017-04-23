using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Models;

namespace Project.Controllers
{
    public class RoleController : MainController
    {
        private readonly RoleManager<IdentityRole> _roles = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

        // GET: Role
        public ActionResult Index()
        {
            return View(DB.Roles);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roles.CreateAsync(model);
                if (result.Succeeded) return PartialView("Row", await _roles.FindByIdAsync(model.Id));
                return PartialView(model);
            }
            return PartialView("Row");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string Id)
        {
            return PartialView("Edit", await _roles.FindByIdAsync(Id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roles.UpdateAsync(model);
                if (result.Succeeded) return PartialView("Row", model);
            }
            return PartialView("Row");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string Id)
        {
            IdentityResult result = await _roles.DeleteAsync(await _roles.FindByIdAsync(Id));
            if (result.Succeeded) return Json(new {Success = true, Id});
            return Json(new {Success = false, result.Errors});
        }

        [HttpPost]
        public ActionResult Members(string Id)
        {
            IdentityRole roles = _roles.FindById(Id);
            if (roles.Name == "Instructors") ViewBag.UsList = DB.Instructors.Select(u => new MemberViewModel {Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, IsMember = roles.Users.Any(r => r.UserId == u.Id)}).ToList();
            else if (roles.Name == "Students") ViewBag.UsList = DB.Students.Select(u => new MemberViewModel {Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, IsMember = roles.Users.Any(r => r.UserId == u.Id)}).ToList();
            return PartialView();
        }
    }
}