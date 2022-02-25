using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

namespace MathTasks.ViewModels
{
    public class IdentityUserEditViewModel
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;
        
        [Display(Name ="Is Administrator")]
        public UserClaim? IsAdmin { get; set; }

        [Display(Name ="Content Editor")]
        public IList<UserClaim>? MathTaskContentEditorClaims { get; set; }
    }

    public class UserClaim
    {
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }
        public bool IsSelected { get; set; }
        public string? DisplayName { get; set; }
    }
}