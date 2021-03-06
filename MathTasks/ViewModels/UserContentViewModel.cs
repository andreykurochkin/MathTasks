using MathTasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.ViewModels {
    /// <summary>
    /// manages content associated with the specific user
    /// </summary>
    public class UserContentViewModel {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public IEnumerable<MathTask> MathTasks { get; set; }
    }
}
