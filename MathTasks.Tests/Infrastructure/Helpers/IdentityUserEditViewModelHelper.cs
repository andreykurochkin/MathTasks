using MathTasks.ViewModels;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace MathTasks.Tests.Infrastructure.Helpers;

public static class IdentityUserEditViewModelHelper
{
    private const string emailDomain = ".com";
    public static IdentityUserEditViewModel GetInstanceWithIsAdminProperty() =>
        new IdentityUserEditViewModel
        {
            Email = new EmailAddresses(emailDomain).GetValue(),
            Id = Guid.NewGuid().ToString(),
            IsAdmin = UserClaimsHelper.GetOne(),
            MathTaskContentEditorClaims = Enumerable.Empty<UserClaim>().ToList()
        };

    internal static IdentityUserEditViewModel GetInstanceWithAllUserClaimProperties() =>
        new IdentityUserEditViewModel
        {
            Email = new EmailAddresses(emailDomain).GetValue(),
            Id = Guid.NewGuid().ToString(),
            IsAdmin = UserClaimsHelper.GetOne(),
            MathTaskContentEditorClaims = UserClaimsHelper.GetManyWithoutIsAdmin().ToArray()
        };
}