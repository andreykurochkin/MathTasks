using MathTasks.Controllers.AlterMathTasks.Queries;
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

        public AlterMathTasksController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index(string tag)
        {
            ViewData["tag"] = tag;
            var query = new GetMathTaskViewModelsQuery { Tag = tag };
            var viewModels = await _mediator.Send(query, HttpContext.RequestAborted);
            return View(viewModels);
            // return View(await _mediator.Send(new GetMathTaskViewModelsQuery(), HttpContext.RequestAborted));
        }

        public async Task<IActionResult> Show(Guid id) =>
            View(await _mediator.Send(new GetMathTaskViewModelByIdQuery { Id = id }, HttpContext.RequestAborted));
    }
}
