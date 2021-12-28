using Kurochkin.Persistence.UnitOfWork;
using MathTasks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MathTasks.Persistent.Repositories;

public class MathTaskRepositoryConfigureOptions : IEFCoreRepositoryConfigureOptions<MathTask, Guid>
{
    public MathTaskRepositoryConfigureOptions()
    {
        Include = (x) => x.Include(_ => _.Tags);
        DisableTracking = true;
        Predicate = (x) => x.Id.Equals(SearchId);
    }
    
    // 1. specify member of this
    // 2. specify new value for this
    // 3. return this

    public Func<IQueryable<MathTask>, IOrderedQueryable<MathTask>>? OrderyBy { get; set; }
    public Func<IQueryable<MathTask>, IIncludableQueryable<MathTask, object>>? Include { get; set; }
    public bool DisableTracking { get; set; }
    public Expression<Func<MathTask, bool>>? Predicate { get; set; }
    public Guid SearchId { get; set; }
}
