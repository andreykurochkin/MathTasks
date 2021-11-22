using MathTasks.Providers.Base;

namespace MathTasks.Providers
{
    public class DefaultEmailProvider : IEmailProvider
    {
        private readonly string _value = "dev@gmail.com";
        public override string ToString() => _value;
    }
}
