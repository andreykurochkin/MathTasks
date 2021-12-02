using System;

namespace MathTasks.ViewModels
{
    public class TagCloudViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? CssClass { get; set; }

        public int Weight { get; set; }
    }
}
