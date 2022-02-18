using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using MathTasks.Data;
using System.Net;
using MathTasks.ViewModels;
using MediatR;
using MathTasks.Controllers.Administration.Commands;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System;
using MathTasks.Authorization;
using System.Security.Claims;

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
        var result = await _mediator.Send(query, HttpContext.RequestAborted);
        return View(result);
    }

    public async Task<IActionResult> Edit(string? id)
    {
        var source = await _userManager.FindByIdAsync(id);
        var query = new CreateIdentityUserEditViewModelCommand(source);
        var result = await _mediator.Send(query, HttpContext.RequestAborted);
        return result is null ? RedirectToAction(nameof(Index)) : View(result);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(IdentityUserEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var query = new UpdateIdentityUserCommand(model);
        var result = await _mediator.Send(query, HttpContext.RequestAborted);
        if (result is null)
        {
            ModelState.AddModelError(string.Empty, "Error on edit user. User was deleted before your update");
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }

    private bool GetIsAdminClaimValue(IList<Claim> claims)
    {
        var claim = claims.FirstOrDefault(_ => _.Type == ClaimsStore.IsAdminClaimType);
        return claim is null ? default(bool) : bool.Parse(claim.Value);
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
        var viewModel = new IdentityUserEditViewModel
        {
            Id = user.Id,
            Email = user.Email,
            //Claims = await _userManager.GetClaimsAsync(user),
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
