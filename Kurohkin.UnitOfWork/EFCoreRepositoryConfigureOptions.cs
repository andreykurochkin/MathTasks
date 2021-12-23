using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Kurochkin.Persistene.UnitOfWork;

public class EFCoreRepositoryConfigureOptions<TEntity, TGuid> where TEntity : class where TGuid : struct
{
    public Expression<Func<TEntity, bool>> Predicate { get; set; } = null!;
    
    public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderyBy { get; set; } = null!;
    
    public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> Include {get;set;} = null!;

    public bool DisableTracking { get; set; } = true;
}
