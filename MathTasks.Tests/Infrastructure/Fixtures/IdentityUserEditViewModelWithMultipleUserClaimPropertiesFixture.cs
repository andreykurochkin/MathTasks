using MathTasks.Tests.Infrastructure.Helpers;
using MathTasks.ViewModels;

namespace MathTasks.Tests.Infrastructure.Fixtures;

public class IdentityUserEditViewModelWithMultipleUserClaimPropertiesFixture
{
    public IdentityUserEditViewModel Create() => IdentityUserEditViewModelHelper.GetInstanceWithAllUserClaimProperties();
}