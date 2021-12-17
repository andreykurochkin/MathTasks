using MathTasks.Mediatr;
using MathTasks.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Controllers {
    public class HomeController : Controller {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> Privacy() {
            await _mediator.Publish(new ErrorNotification(content: "Privacy notification test"), HttpContext.RequestAborted);
            await _mediator.Publish(new FeedBackNotification(content: "Privacy notification test"), HttpContext.RequestAborted);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
