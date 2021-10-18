using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Models {
    public class MathTask {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Theme { get; set; }
        public string Topic { get; set; }
        public string Tags { get; set; }
        public MathTask() {

        }
    }
}
