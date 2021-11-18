using System;
using System.Collections.Generic;

namespace MathTasks.Models
{
    public class Tag
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public ICollection<MathTask> MathTasks { get; set; }
    }
}
