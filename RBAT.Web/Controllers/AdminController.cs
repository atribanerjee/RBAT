using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Web.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RBAT.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users;
            var userRoles = new List<UserRolesModel>();
            foreach(var user in users)
            {
                userRoles.Add(new UserRolesModel
                {
                    Id = new Guid(user.Id),
                    UserName = user.UserName,
                    IsSubscriber = await _userManager.IsInRoleAsync(user, "Subscriber")
                });
            }
            var model = new AdminModel
            {
                Users = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> SaveUserRoles(AdminModel adminModel)
        {
            try
            {
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var alreadyASubscriber = await _userManager.IsInRoleAsync(user, "Subscriber");
                    var updatedUser = adminModel.Users.First(u => u.Id == new Guid(user.Id));

                    await _userManager.UpdateAsync(user);

                    if (updatedUser.IsSubscriber && !alreadyASubscriber)
                    {
                        await _userManager.AddToRoleAsync(user, "Subscriber");
                    }
                    else if (!updatedUser.IsSubscriber && alreadyASubscriber)
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Subscriber");
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Type = "Error", Message = ex.Message });
            }
            return Json(new { Type = "Success" });
        }
    }
}