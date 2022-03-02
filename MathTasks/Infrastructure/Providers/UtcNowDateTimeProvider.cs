using MathTasks.Infrastructure.Providers.Base;
using System;

namespace MathTasks.Infrastructure.Providers;

public class UtcNowDateTimeProvider : IDateTimeProvider
{
    private readonly DateTime value = DateTime.UtcNow;
    public DateTime ToDateTime() => value;
}
