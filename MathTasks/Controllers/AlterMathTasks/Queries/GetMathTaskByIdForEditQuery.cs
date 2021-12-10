using MathTasks.ViewModels;
using MediatR;
using System;

namespace MathTasks.Controllers.AlterMathTasks.Queries
{
    public record GetMathTaskByIdForEditQuery(Guid Id, string ReturnUrl=null!) : IRequest<MathTaskEditViewModel>;
}
