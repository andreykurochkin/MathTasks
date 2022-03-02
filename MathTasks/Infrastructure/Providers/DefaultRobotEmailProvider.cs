using MathTasks.Infrastructure.Providers.Base;

namespace MathTasks.Infrastructure.Providers;

public class DefaultRobotEmailProvider : IEmailProvider
{
    private readonly string _value = "noreply@gmail.com";
    public override string ToString() => _value;
}
