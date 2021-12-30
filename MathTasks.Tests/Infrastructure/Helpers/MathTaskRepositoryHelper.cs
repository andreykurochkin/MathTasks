using Kurochkin.Persistence.UnitOfWork;
using MathTasks.Models;
using Moq;
using System;

namespace MathTasks.Tests.Infrastructure.Helpers;

public class MathTaskRepositoryHelper
{
    public static Mock<IRepository<MathTask,Guid>> GetMock()
    {
        var context = new DbContextHelper().Context;
        var mathTaskRepository = new Mock<IRepository<MathTask, Guid>>(context);
        return mathTaskRepository;
    }
}
