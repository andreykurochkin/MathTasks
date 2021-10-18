using MathTasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.ViewModels
{
    public class CreateMathTaskViewModel
    {
        public IEnumerable<Topic> Topics { get; set; }
        public CreateMathTaskViewModel() {

        }
    }
}
