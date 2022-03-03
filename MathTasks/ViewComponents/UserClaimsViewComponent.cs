using MathTasks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathTasks.ViewComponents;

public class UserClaimsViewComponent : ViewComponent
{
    //private readonly IEnumerable<UserClaim> _userClaims;

    //public UserClaimsViewComponent(IEnumerable<UserClaim> userClaims)
    //{
    //    _userClaims = userClaims;
    //}
    public UserClaimsViewComponent()
    {

    }
    public async Task<IViewComponentResult> InvokeAsync(IEnumerable<UserClaim> _userClaims) => await Task.FromResult(View(_userClaims));
    //public IViewComponentResult Invoke() => View();
}
