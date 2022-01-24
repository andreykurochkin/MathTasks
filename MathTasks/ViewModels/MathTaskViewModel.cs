using MathTasks.Models;
using System;
using System.Collections.Generic;

namespace MathTasks.ViewModels
{
    public class MathTaskViewModel
    {
        public Guid Id { get; set; }
        public string Theme { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get;set; }
        public IEnumerable<TagViewModel> Tags { get; set; } = null!;
    }
}