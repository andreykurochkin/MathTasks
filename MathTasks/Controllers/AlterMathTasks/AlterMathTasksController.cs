using MathTasks.Controllers.AlterMathTasks.Queries;
using MathTasks.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Controllers.AlterMathTasks
{
    public class AlterMathTasksController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ITagService _tagService;
        private readonly IJSRuntime _jSRuntime;

        public AlterMathTasksController(IMediator mediator, ITagService tagService, IJSRuntime jSRuntime)
        {
            (_mediator, _tagService) = (mediator, tagService);
            _jSRuntime = jSRuntime;
        }

        public async Task<IActionResult> Index(string tag)
        {
            ViewData["tag"] = tag;
            var query = new GetMathTaskViewModelsQuery { Tag = tag };
            var viewModels = await _mediator.Send(query, HttpContext.RequestAborted);
            return View(viewModels);
        }

        public async Task<IActionResult> Show(Guid id) => 
            View(await _mediator.Send(new GetMathTaskViewModelByIdQuery { Id = id }, HttpContext.RequestAborted));

        public IActionResult Cloud() => View();

        //public async Task<IActionResult> CloudViaView() => View(await _tagService.GetCloudAsync());
        public async Task<IActionResult> ActionResult()
        {
            var interop = new RazorLibrary.RazorInterop(_jSRuntime);
            return View(interop);
        }
    }
}
