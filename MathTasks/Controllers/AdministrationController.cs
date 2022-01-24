using Microsoft.AspNetCore.Mvc;

namespace MathTasks.Controllers;

//[Authorize(Roles ="Administrator")]
public class AdministrationController : Controller
{
    public IActionResult CreateRole() => View();







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
