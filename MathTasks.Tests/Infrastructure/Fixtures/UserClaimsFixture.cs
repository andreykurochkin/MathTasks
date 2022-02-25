using MathTasks.Tests.Infrastructure.Helpers;
using MathTasks.ViewModels;
using System.Collections.Generic;

namespace MathTasks.Tests.Infrastructure.Fixtures;

public class UserClaimsFixture
{
    public IEnumerable<UserClaim> Create() => new List<UserClaim>(UserClaimsHelper.GetMany());
}