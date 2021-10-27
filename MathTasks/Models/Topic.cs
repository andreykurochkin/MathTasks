using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Models {
    public class Topic {
        public int Id { get; set; }
        public string Content { get; set; }
        public IEnumerable<MathTask> MathTasks { get; set; } = new List<MathTask>();
    }
}
