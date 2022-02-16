using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;
using System.Threading.Tasks;
using MathTasks.Data;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MathTasks.ViewModels;

namespace MathTasks.Controllers;

//[Authorize(Roles ="Administrator")]
public class AdministrationController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AdministrationController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Index() => View(_userManager.Users);

    public async Task<IActionResult> DeleteUser(string? id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("NotFoundIdentityUser");
        }
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            result.Errors.ToList().ForEach(_ => ModelState.AddModelError(String.Empty, $"code: {_.Code}; description: {_.Description}"));
            return View("ListUsers", _userManager.Users);
        }
        return View("ListUsers", _userManager.Users);
    }

    [HttpGet]
    public async Task<IActionResult> EditUser(string? id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("NotFoundIdentityUser");
        }
        var viewModel = new EditUserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            Claims = await _userManager.GetClaimsAsync(user),
        };
        return View(viewModel);
    }

    //private readonly UserManager<IdentityUser> _userManager;





    //public AdministrationController(UserManager<IdentityUser> userManager) {
    //    _userManager = userManager;
    //}

    //public IActionResult Index() {
    //    var users = _userManager.Users;
    //    return View(users);
    //}

    //[HttpGet]
    //public async Task<IActionResult> AcquireUserContent(string userId) {
    //    var user = await _userManager.FindByIdAsync(userId);
    //    return (user is null)
    //        ? NotFound()
    //        : View(CreateUserContentViewModelWithMockMathTasks(user));
    //}

    //private UserContentViewModel CreateUserContentViewModelWithMockMathTasks(IdentityUser user) {
    //    var viewModel = new UserContentViewModel() { UserId = user.Id, UserEmail = user.Email, MathTasks = new MockMathTasks() };
    //    return viewModel;
    //}
}
