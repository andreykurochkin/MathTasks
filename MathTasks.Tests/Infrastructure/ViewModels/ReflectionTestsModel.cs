using MathTasks.ViewModels;
using System.Collections.Generic;

namespace MathTasks.Tests.Infrastructure.ViewModels;

public class ReflectionTestsModel
{
    public UserClaim? FirstItem { get; set; }
    public UserClaim? SecondItem { get; set; }
    public IList<UserClaim>? FirstCollection { get; set; }
    public IList<UserClaim>? SecondCollection { get; set; }
}
