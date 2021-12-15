using MathTasks.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MathTasks.ViewModels
{
    public class MathTaskEditViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Theme of Math Task")]
        public string Theme { get; set; } = null!;

        [Display(Name = "Theme of Math Task")]
        public string Content { get; set; } = null!;

        public string ReturnUrl { get; set; } = null!;

        [Display(Name ="Tags of Math Task")]
        public IEnumerable<string>? Tags { get; set; }

        [Display(Name = "Tags of Math Task")]
        [Range(1,8, ErrorMessage ="Up to 8 Tags needed")]
        public int TotalTags { get; set; }
    }
}
