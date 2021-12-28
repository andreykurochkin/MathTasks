using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Kurochkin.Persistence.UnitOfWork;

public interface IEFCoreRepositoryConfigureOptions<TEntity, TGuid> where TEntity : class where TGuid : struct 
{
    public TGuid SearchId { get; set; }
    public Expression<Func<TEntity, bool>>? Predicate { get; set; }

    public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderyBy { get; set; }

    public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? Include { get; set; }

    public bool DisableTracking { get; set; }
}
