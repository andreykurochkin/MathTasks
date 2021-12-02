using MathTasks.Controllers.AlterMathTasks.Queries;
using MathTasks.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Controllers.AlterMathTasks
{
    public class AlterMathTasksController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ITagService _tagService;

        public AlterMathTasksController(IMediator mediator, ITagService tagService) => (_mediator, _tagService) = (mediator, tagService);

        public async Task<IActionResult> Index() => 
            View(await _mediator.Send(new GetMathTaskViewModelsQuery(), HttpContext.RequestAborted));

        public async Task<IActionResult> Show(Guid id) => 
            View(await _mediator.Send(new GetMathTaskViewModelByIdQuery { Id = id }, HttpContext.RequestAborted));

        public IActionResult Cloud() => View();

        public async Task<IActionResult> CloudViaView() => View(await _tagService.GetCloudAsync());
    }
}
