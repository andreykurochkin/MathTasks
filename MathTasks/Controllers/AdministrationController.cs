using MathTasks.Data;
using MathTasks.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Controllers {
    [Authorize(Roles ="Administrator")]
    public class AdministrationController : Controller {
        private readonly UserManager<IdentityUser> _userManager;

        public AdministrationController(UserManager<IdentityUser> userManager) {
            _userManager = userManager;
        }

        public IActionResult Index() {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> AcquireUserContent(string userId) {
            var user = await _userManager.FindByIdAsync(userId);
            return (user is null)
                ? NotFound()
                : View(CreateUserContentViewModelWithMockMathTasks(user));
        }

        private UserContentViewModel CreateUserContentViewModelWithMockMathTasks(IdentityUser user) {
            var viewModel = new UserContentViewModel() { UserId = user.Id, UserEmail = user.Email, MathTasks = new MockMathTasks() };
            return viewModel;
        }
    }
}
