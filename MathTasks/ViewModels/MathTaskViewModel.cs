using MathTasks.Models;
using System;
using System.Collections.Generic;

namespace MathTasks.ViewModels
{
    public class MathTaskViewModel
    {
        public Guid Id { get; set; }
        public string Theme { get; set; }
        public string Content { get; set; }
        public IEnumerable<TagViewModel> Tags { get; set; }
    }
}