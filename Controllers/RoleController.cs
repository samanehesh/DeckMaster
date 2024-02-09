using DECKMASTER.Data;
using DECKMASTER.Repositories;
using DECKMASTER.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DECKMASTER.Controllers

{
    [Authorize(Roles = "Admin,Manager")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoleController(ApplicationDbContext db)
        {
            _db = db;
        }

        public ActionResult Index(string message ="")
        {
            ViewBag.Message = message;
            RoleRepo roleRepo = new RoleRepo(_db);
            return View(roleRepo.GetAllRoles());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                RoleRepo roleRepo = new RoleRepo(_db);
                bool isSuccess = roleRepo.CreateRole(roleVM.RoleName);

                if (isSuccess)
                {
                    string message = $"{roleVM.RoleName} Role created successfully.";
                    return RedirectToAction("Index", "Role", new { message = message }); // Redirect to the Role controller's Index action
                }
                else
                {
                    ModelState.AddModelError("", "Role creation failed.");
                    ModelState.AddModelError("", "The role may already exist.");
                }
            }

            return View(roleVM);
        }



        [HttpGet]
        public ActionResult Delete(string roleName)
        {
            RoleRepo roleRepo = new RoleRepo(_db);

            RoleVM roleVM = roleRepo.GetRole(roleName);

            return View(roleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(RoleVM roleVM)
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            string deletMessage = roleRepo.DeleteRole(roleVM.RoleName); // Assuming Id property in RoleVM
            if (!deletMessage.Contains("Error!"))
            {
                // Redirect to a success page or another appropriate action
                return RedirectToAction("Index", "Role", new { message = deletMessage }); // Redirect to the Role controller's Index action
            }
            else
            {
                // Optionally, you can pass an error message to the view
                

                // Return the same view with the provided ViewModel
                return RedirectToAction("Index", "Role", new { message = deletMessage });
            }
        }

    }
}
