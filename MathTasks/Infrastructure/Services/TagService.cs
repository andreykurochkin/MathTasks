using MathTasks.Data;
using MathTasks.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TagService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<TagCloudViewModel>> GetCloudAsync()
        {
            var viewModels = (await _applicationDbContext.Tags
                .Include(t => t.MathTasks)
                .ToListAsync())
                .Select(entity => new TagCloudViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    CssClass = string.Empty,
                    Weight = entity.MathTasks == null ? 0 : entity.MathTasks.Count
                });
            return viewModels;
        }
    }
}
