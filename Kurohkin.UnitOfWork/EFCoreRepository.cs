using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Linq;

namespace Kurochkin.Persistene.UnitOfWork;
public class EFCoreRepository<TEntity, TGuid> : IRepository<TEntity, TGuid> where TEntity : class where TGuid : struct
{
    protected DbContext DbContext { get; }
    protected DbSet<TEntity> DbSet { get; }

    public EFCoreRepository(DbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = dbContext.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> prediate)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> Get(TGuid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TEntity> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Remove(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderyBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = DbSet;
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }
        if (include is not null)
        {
            query = include(query);
        }
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        return orderyBy is null
            ? query.FirstOrDefaultAsync()
            : orderyBy(query).FirstOrDefaultAsync();

    }

    public virtual async Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderyBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = DbSet;
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }
        if (include is not null)
        {
            query = include(query);
        }
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        return orderyBy is null
            ? await query.Select(selector).FirstOrDefaultAsync()
            : await orderyBy(query).Select(selector).FirstOrDefaultAsync();

    }
}