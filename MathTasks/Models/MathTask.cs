using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Models {
    public class MathTask {
        public Guid Id { get; set; }
        public string Theme { get; set; }
        public string Content { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public MathTask() {

        }
    }
}
