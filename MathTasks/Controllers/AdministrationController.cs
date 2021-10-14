using MathTasks.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Controllers {
    public class AdministrationController : Controller {
        private readonly UserManager<IdentityUser> _userManager;

        public AdministrationController(UserManager<IdentityUser> userManager) {
            _userManager = userManager;
        }

        public IActionResult Index() {
            var users = _userManager.Users;
            return View(users);
        }
    }
}
