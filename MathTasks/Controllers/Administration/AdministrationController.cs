using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using MathTasks.Data;
using System.Net;
using MathTasks.ViewModels;
using MediatR;
using MathTasks.Controllers.Administration.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MathTasks.Controllers.Administration;

//[Authorize(Roles ="Administrator")]
public class AdministrationController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public AdministrationController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IMediator mediator)
    {
        _userManager = userManager;
        _context = context;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var source = await _userManager.Users.ToListAsync();
        var query = new GetIdentityUserViewModelsQuery(source);
        var result = await _mediator.Send(query);
        return View(result);
    }

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
            result.Errors.ToList().ForEach(_ => ModelState.AddModelError(string.Empty, $"code: {_.Code}; description: {_.Description}"));
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
