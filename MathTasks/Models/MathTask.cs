using MathTasks.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Models
{
    public class MathTask : Auditable
    {
        public Guid Id { get; set; }
        public string Theme { get; set; } = null!;

        public string Content { get; set; } = null!;
        public ICollection<Tag> Tags { get; set; } = null!;
        public MathTask()
        {

        }
    }
}
