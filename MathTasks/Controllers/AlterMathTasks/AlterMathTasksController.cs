﻿using MathTasks.Controllers.AlterMathTasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MathTasks.Test
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

        }
    }
}

namespace MathTasks.Controllers.AlterMathTasks
{
    public class AlterMathTasksController : Controller
    {
        private readonly IMediator _mediator;

        public AlterMathTasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string Index()
        {
            return "Hello World";
            //var models = await _mediator.Send(new GetMathTaskViewModelsQuery());
            //return View(models);
        }
    }
}