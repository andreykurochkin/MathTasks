using AutoMapper;
using MathTasks.Controllers.AlterMathTasks.Commands;
using MathTasks.Controllers.AlterMathTasks.Queries;
using MathTasks.Data;
using MathTasks.Infrastructure.Services;
using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Controllers.AlterMathTasks
{
    public class AlterMathTasksController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ITagService _tagService;
        private readonly IJSRuntime _jSRuntime;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AlterMathTasksController(IMediator mediator,
                                        ITagService tagService,
                                        IJSRuntime jSRuntime,
                                        ApplicationDbContext context,
                                        IMapper mapper)
        {
            (_mediator, _tagService) = (mediator, tagService);
            _jSRuntime = jSRuntime;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var fastStartup = false;
            if (fastStartup)
            {
                return View(Enumerable.Empty<MathTaskViewModel>());
            }
            ViewData["tag"] = tag;
            var query = new GetMathTaskViewModelsQuery(tag, string.Empty);/* { Tag = tag };*/
            var viewModels = await _mediator.Send(query, HttpContext.RequestAborted);
            return View(viewModels);
        }

        public async Task<IActionResult> Show(Guid id) =>
            View(await _mediator.Send(new MathTaskGetByIdQuery(id), HttpContext.RequestAborted));

        public IActionResult Cloud() => View();

        //public async Task<IActionResult> CloudViaView() => View(await _tagService.GetCloudAsync());

        public IActionResult ActionResult()
        {
            var interop = new RazorLibrary.RazorInterop(_jSRuntime);
            return View(interop);
        }

        public async Task<IActionResult> Edit(Guid id, string returnUrl)
        {
            var result = await _mediator.Send(new GetMathTaskByIdForEditQuery(id, returnUrl));
            return (result is null)
                ? View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier })
                : View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MathTaskEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new UpdateMathTaskCommand(model));
                if (result is not null)
                {
                    return Redirect(model.ReturnUrl);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new MathTaskCreateViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MathTaskCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new CreateMathTaskCommand(viewModel));
                if (result is not null)
                {
                    return RedirectToAction("Index", "AlterMathTasks");
                }
                ModelState.AddModelError("", "error on create new math task operation");
            }
            return View(viewModel);
        }
    }
}
