using MathTasks.ViewModels;
using System.Collections.Generic;

namespace MathTasks.Tests.Infrastructure.ViewModels;

public class ModelWithTwoUserClaimCollections
{
    public IList<UserClaim>? FirstCollection { get; set; }
    public IList<UserClaim>? SecondCollection { get; set; }
}
