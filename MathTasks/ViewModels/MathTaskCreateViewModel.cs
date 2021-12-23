using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.ViewModels;

public class MathTaskCreateViewModel
{
    [Display(Name = "Theme of Math Task")]
    public string Theme { get; set; } = null!;

    [Display(Name = "Theme of Math Task")]
    public string Content { get; set; } = null!;
    public List<string> Tags { get; set; } = new List<string>();

    [Display(Name = "Tags of Math Task")]
    [Range(1, 8, ErrorMessage = "Up to 8 Tags needed")]
    public int TotalTags { get; set; }

}
