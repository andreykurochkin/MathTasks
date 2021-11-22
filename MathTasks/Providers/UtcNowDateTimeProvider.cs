using MathTasks.Providers.Base;
using System;

namespace MathTasks.Providers
{
    public class UtcNowDateTimeProvider : IDateTimeProvider
    {
        private DateTime _value => DateTime.UtcNow;
        public DateTime ToDateTime() => _value;
    }
}
