using MathTasks.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Controllers {
    [Authorize(Roles = "Administrator")]
    public class RolesController : Controller {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        public IActionResult Create() {
            return View();
        }


        private Task<IdentityResult> CreateRole(string roleName) {
            return _roleManager.CreateAsync(new IdentityRole(roleName));
        }
        private void AddErrorsToModel(IdentityResult identityResult) {
            identityResult.Errors.ToList()
                .ForEach(error => 
                    ModelState.AddModelError(string.Empty, error.Description)
                );
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name) {
            var result = await CreateRole(name);
            if (result.Succeeded) {
                return RedirectToAction("Index");
            }
            AddErrorsToModel(result);
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id) {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role is not null) {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string userId) {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) {
                return NotFound();
            }
            return View(await CreateChangeRoleViewModel(user));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, IEnumerable<string> roles) {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
            return RedirectToAction("UserList");
        }

        private async Task<ChangeRoleViewModel> CreateChangeRoleViewModel(IdentityUser user) {
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles;
            var changeRoleViewModel = new ChangeRoleViewModel() { UserId = user.Id, UserRoles = userRoles, AllRoles = allRoles.ToList(), UserEmail = user.Email };
            return changeRoleViewModel;
        }


        public IActionResult UserList() {
            var users = _userManager.Users;
            return View(users);
        }
    }
}
