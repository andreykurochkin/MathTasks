using MathTasks.Models;
using MathTasks.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Infrastructure.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagCloudViewModel>> GetCloudAsync();
        Task UpdateTagsInDatabaseAsync(IEnumerable<string> tagNamesFromModel, MathTask mathTask, CancellationToken cancellationToken);
    }
}
