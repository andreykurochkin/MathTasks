using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Controllers.AlterMathTasks.Commands
{
    public record CreateMathTaskCommand(MathTaskCreateViewModel viewModel) : IRequest<MathTask>;
}
