using System.Collections.Generic;
using System.Security.Claims;

namespace MathTasks.Authorization;

public static class ClaimsStore
{
    public const string IsAdminClaimType = "IsAdmin";
    public static IEnumerable<Claim> AllClaims()
    {
        yield return new Claim(IsAdminClaimType, "False");
    }
}