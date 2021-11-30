using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
using System;

namespace MathTasks.Controllers.AlterMathTasks.Queries
{
    public record GetMathTaskViewModelByIdQuery : IRequest<MathTaskViewModel>
    {
        public Guid Id { get; init;}
    };
}
