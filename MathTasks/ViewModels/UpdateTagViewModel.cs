using System;
using System.Collections.Generic;

namespace MathTasks.ViewModels
{
    public class UpdateTagViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> MathTasks { get; set; }
    }
}