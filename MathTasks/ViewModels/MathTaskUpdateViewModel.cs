using System;
using System.Collections.Generic;

namespace MathTasks.ViewModels
{
    internal class MathTaskUpdateViewModel
    {
        public Guid Id { get; set; }
        public string Theme { get; set; }
        public string Content { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}
