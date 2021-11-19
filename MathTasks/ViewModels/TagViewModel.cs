using System;
using System.Collections.Generic;

namespace MathTasks.ViewModels
{
    public class TagViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<MathTaskViewModel> MathTasks { get; set; }
    }
}