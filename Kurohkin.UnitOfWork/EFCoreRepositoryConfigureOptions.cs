using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Kurochkin.Persistence.UnitOfWork;

public class EFCoreRepositoryConfigureOptions<TEntity, TGuid> : IEFCoreRepositoryConfigureOptions<TEntity, TGuid> where TEntity : class where TGuid : struct
{
    // todo delete class
    // I use specific classes in MathTask project Persistent folder instead of that POCO
    public EFCoreRepositoryConfigureOptions()
    {

    }
    public Expression<Func<TEntity, bool>>? Predicate { get ; set; }
    public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderyBy { get; set; }
    public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? Include { get; set; }
    public bool DisableTracking { get; set; }
    public TGuid SearchId { get; set; }
}
