using System.Collections.Generic;
using System.Security.Claims;

namespace MathTasks.Authorization;

public static class ClaimsStore
{
    public const string IsAdminClaimType = "IsAdmin";
    public const string CanCreateMathTask = "CanCreateMathTask";
    public const string CanReadMathTask = "CanReadMathTask";
    public const string CanUpdateMathTask = "CanUpdateMathTask";
    public const string CanDeleteMathTask = "CanDeleteMathTask";

    public static IEnumerable<string> GetAllClaimTypes()
    {
        yield return IsAdminClaimType;
        yield return CanCreateMathTask;
        yield return CanReadMathTask;
        yield return CanUpdateMathTask;
        yield return CanDeleteMathTask;

    }

    public static IEnumerable<Claim> AllClaims()
    {
        yield return new Claim(IsAdminClaimType, "False");
        yield return new Claim(CanCreateMathTask, "False");
        yield return new Claim(CanReadMathTask, "False");
        yield return new Claim(CanUpdateMathTask, "False");
        yield return new Claim(CanDeleteMathTask, "False");

    }
}