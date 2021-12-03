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


            var foo = new TagCloudViewModelCluster(9, (t) => t.Weight, new List<TagCloudViewModel> { new() });
        }
    }

    public class ClusterOptions<T>
    {

    }

    /// <summary>
    /// Generates cluster
    /// for each instance of TagCloudViewModel creates a number
    /// numbers belong to the range from 0 to count - 1
    /// </summary>
    public class TagCloudViewModelCluster
    {
        private readonly int _startIndex = 0;
        public TagCloudViewModelCluster(int endIndex, Func<TagCloudViewModel, int> onMember, IEnumerable<TagCloudViewModel> items)
        {

        }
    }
}
