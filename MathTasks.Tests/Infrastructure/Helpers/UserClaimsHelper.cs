using MathTasks.Authorization;
using MathTasks.ViewModels;
using System;
using System.Collections.Generic;

namespace MathTasks.Tests.Infrastructure.Helpers;

public static class UserClaimsHelper
{
    public static UserClaim GetOne(string claimType = ClaimsStore.IsAdminClaimType) =>
        new UserClaim
        {
            ClaimType = claimType,
            ClaimValue = Guid.NewGuid().ToString(),
            DisplayName = String.Empty,
            IsSelected = false
        };

    public static IEnumerable<UserClaim> GetMany()
    {
        yield return GetOne();
        yield return GetOne(ClaimsStore.CanCreateMathTask);
    }
}