using MathTasks.Providers.Base;
using System;

namespace MathTasks.Providers
{
    public class UtcNowDateTimeProvider : IDateTimeProvider
    {
        private readonly DateTime value = DateTime.UtcNow;
        public DateTime ToDateTime() => value;
    }
}
