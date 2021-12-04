using System;
using System.Collections.Generic;

namespace MathTasks.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<MathTask>? MathTasks { get; set; }
    }
}
