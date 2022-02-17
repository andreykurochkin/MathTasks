using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MathTasks.ViewModels;

/// <summary>
/// manages roles within single user
/// </summary>
public class ChangeRoleViewModel
{
    public string UserId { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public List<IdentityRole> AllRoles { get; set; }
    public IEnumerable<string> UserRoles { get; set; }
    public ChangeRoleViewModel()
    {
        AllRoles = new List<IdentityRole>();
        UserRoles = new List<string>();
    }
}
