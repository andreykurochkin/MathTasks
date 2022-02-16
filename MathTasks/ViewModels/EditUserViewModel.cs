using System.Collections.Generic;
using System.Security.Claims;

namespace MathTasks.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IList<Claim>? Claims { get; set; }
    }
}