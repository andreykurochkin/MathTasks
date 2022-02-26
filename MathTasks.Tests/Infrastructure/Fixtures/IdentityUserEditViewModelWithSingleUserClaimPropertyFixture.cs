using MathTasks.Tests.Infrastructure.Helpers;
using MathTasks.Tests.Infrastructure.ViewModels;
using MathTasks.ViewModels;
using System.Linq;

namespace MathTasks.Tests.Infrastructure.Fixtures;

public class IdentityUserEditViewModelWithSingleUserClaimPropertyFixture
{
    public IdentityUserEditViewModel Create() => IdentityUserEditViewModelHelper.GetInstanceWithIsAdminProperty();
}

public class ModelWithTwoUserClaimCollectionsFixture
{
    public ModelWithTwoUserClaimCollections Create() => new ModelWithTwoUserClaimCollections
    {
        FirstCollection = UserClaimsHelper.GetMany().ToList(),
        SecondCollection = UserClaimsHelper.GetManyWithoutIsAdmin().ToList()
    };
}
