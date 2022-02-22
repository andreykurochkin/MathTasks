using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Authorization.Security;

public class IsAdminClaimIsFreeForAllHandler : AuthorizationHandler<ManageIsAdminClaimRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageIsAdminClaimRequirement requirement)
    {
        var claims = context.User.Claims;
        var claim = claims.FirstOrDefault(_ => _.Type == ClaimsStore.IsAdminClaimType);
        if (claim == null)
        {
            return Task.CompletedTask;
        }
        var claimValueIsValid = claim.Value == "True";
        if (!claimValueIsValid)
        {
            return Task.CompletedTask;
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
