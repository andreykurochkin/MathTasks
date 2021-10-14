using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MathTasks.ViewModels {
    /// <summary>
    /// manages roles within single user
    /// </summary>
    public class ChangeRoleViewModel {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public ChangeRoleViewModel() {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
