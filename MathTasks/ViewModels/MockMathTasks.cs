using MathTasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.ViewModels {
    public class MockMathTasks : List<MathTask> {
        public MockMathTasks() {
            for (int i = 0; i < 5; i++) {
                this.Add(new MathTask() { Id=i, Name = DateTime.UtcNow.ToString() });
            }
        }
    }
}
