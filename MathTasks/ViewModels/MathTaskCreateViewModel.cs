using System.Collections.Generic;

namespace MathTasks.ViewModels
{
    public class MathTaskCreateViewModel
    {
        public string Theme { get; set; }
        public string Content { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}