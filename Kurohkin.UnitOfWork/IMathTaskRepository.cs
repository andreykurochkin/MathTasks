using Kurochkin.Persistene.UnitOfWork;
using MathTasks.Models;
using System.Threading.Tasks;

namespace Kurohkin.Persistene.UnitOfWork;

public interface IMathTaskRepository : IRepository<MathTask, Guid>
{
    Task<MathTask?> GetWithTagsAsync(Guid id, CancellationToken token);
}
