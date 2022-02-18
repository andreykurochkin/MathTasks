using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MathTasks.ViewModels
{
    public class EditIdentityUserViewModel
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;
        
        [Display(Name ="Is Administrator")]
        public bool IsAdmin { get; set; }
    }
}