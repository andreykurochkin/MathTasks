using MathTasks.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathTasks.Infrastructure.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagCloudViewModel>> GetCloudAsync();
    }
}
