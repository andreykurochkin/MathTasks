using MathTasks.Tests.Infrastructure.Helpers;
using MathTasks.Data;

namespace MathTasks.Tests.Infrastructure.Fixtures;

public class ApplicationDBContextFixture
{
    public ApplicationDbContext Create() => new DbContextHelper().Context;
}