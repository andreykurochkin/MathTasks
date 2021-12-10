using MathTasks.Models;
using System.Collections.Generic;

namespace MathTasks.ViewModels
{
    public class MathTaskEditViewModel
    {
        public string Theme { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string ReturnUrl { get; set; }=null!;

        public IEnumerable<Tag> Tags { get; set; }

        public int TotalTags { get; set; }
    }
}
