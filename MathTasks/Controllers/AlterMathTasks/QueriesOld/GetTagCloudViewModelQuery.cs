using MathTasks.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace MathTasks.Controllers.AlterMathTasks.Queries
{
    public record GetTagCloudViewModelQuery : IRequest<IEnumerable<TagCloudViewModel>>;
}