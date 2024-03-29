﻿using DECKMASTER.Data;
using DECKMASTER.Repositories;
using DECKMASTER.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DECKMASTER.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class UserRoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRoleController(ApplicationDbContext context,
                                 UserManager<IdentityUser> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            UserRepo userRepo = new UserRepo(_db);
            IEnumerable<UserVM> users = userRepo.GetAllUsers();

            return View(users);
        }

        public async Task<IActionResult> Detail(string userName, string message = "")
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);
            MyRegisteredUserRepo myRegisteredUserRepo = new MyRegisteredUserRepo(_db);
            var FullName = myRegisteredUserRepo.GetFullNameByEmail(userName);

            var roles = await userRoleRepo.GetUserRolesAsync(userName);
            ViewBag.FullName = FullName;
            ViewBag.Message = message;
            ViewBag.UserName = userName;

            return View(roles);
        }

        public ActionResult Create()
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.RoleSelectList = roleRepo.GetRoleSelectList();


            UserRepo userRepo = new UserRepo(_db);
            ViewBag.UserSelectList = userRepo.GetUserSelectList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);

            if (ModelState.IsValid)
            {
                try
                {
                    var addUR =
                    await userRoleRepo.AddUserRoleAsync(userRoleVM.Email,
                                                        userRoleVM.RoleName);

                    string message = $"{userRoleVM.RoleName} permissions" +
                                     $" successfully added to " +
                                     $"{userRoleVM.Email}.";

                    return RedirectToAction("Detail", "UserRole",
                                      new
                                      {
                                          userName = userRoleVM.Email,
                                          message = message
                                      });
                }
                catch
                {
                    ModelState.AddModelError("", "UserRole creation failed.");
                    ModelState.AddModelError("", "The Role may exist " +
                                                 "for this user.");
                }
            }

            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.RoleSelectList = roleRepo.GetRoleSelectList();

            UserRepo userRepo = new UserRepo(_db);
            ViewBag.UserSelectList = userRepo.GetUserSelectList();

            return View();
        }


        public ActionResult Delete(string roleName,string userName)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);
            var userRoleVM = new UserRoleVM { RoleName = roleName, Email = userName };

            return View(userRoleVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);
            var success = await userRoleRepo.RemoveUserRoleAsync(userRoleVM.Email, userRoleVM.RoleName);
            if (success)
            {
                return RedirectToAction("Detail", "UserRole",
                        new { userName = userRoleVM.Email, message = $"{userRoleVM.RoleName} Role deleted successfully for {userRoleVM.Email}." });
            }

            ModelState.AddModelError("", "User Role deletion failed.");
            
            return View(userRoleVM);
        }
    }

    

}


    

