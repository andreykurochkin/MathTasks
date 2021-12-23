using Kurochkin.Persistene.UnitOfWork;
using MathTasks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Persistent.Repositories
{
    public class MathTaskRepositoryConfigureOptions
    {
        public Func<IQueryable<MathTask>, IIncludableQueryable<MathTask, object>> IncludeTags { get; set; } = null!;
    }

    public class MathTaskRepository : EFCoreRepository<MathTask, Guid>
    {
        private readonly MathTaskRepositoryConfigureOptions _options = new();

        public MathTaskRepository(DbContext context, Action<MathTaskRepositoryConfigureOptions>? configureOptions) : base(context)
        {
            // _includeTags = (x) => x.Include(x=>x.Tags);
            if (configureOptions is not null)
            {
                configureOptions(_options);
            }
        }

        public Task<MathTask> Get() => GetFirstOrDefaultAsync<MathTask>(include: _options.IncludeTags);
    }
}
