using System;
using System.Collections.Generic;

namespace MathTasks.ViewModels
{
    internal class MathTaskUpdateViewModel
    {
        public Guid Id { get; set; }
        public string Theme { get; set; } = null!;
        public string Content { get; set; } = null!;
        public ICollection<string> Tags { get; set; } = null!;
    }
}
