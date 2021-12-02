using MathTasks.Infrastructure.Services;
using MathTasks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathTasks.ViewComponents
{
    public class CloudViewComponent : ViewComponent
    {
        private readonly ITagService _tagService;

        public CloudViewComponent(ITagService tagService) => _tagService = tagService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _tagService.GetCloudAsync();
            return View(tags);
        }
    }
}
