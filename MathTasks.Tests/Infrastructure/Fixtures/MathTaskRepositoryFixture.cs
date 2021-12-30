using MathTasks.Tests.Infrastructure.Helpers;
using MathTasks.Persistent.Repositories;

namespace MathTasks.Tests.Infrastructure.Fixtures;

public class MathTaskRepositoryFixture
{
    public MathTaskRepository Create()
    {
        var context = new DbContextHelper().Context;
        return new MathTaskRepository(context);
    }
}
