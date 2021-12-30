using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Linq;

namespace Kurochkin.Persistence.UnitOfWork;
public class EFCoreRepository<TEntity, TGuid> : IRepository<TEntity, TGuid> where TEntity : class where TGuid : struct
{
    protected TGuid Guid { get; set; }
    protected DbContext DbContext { get; }
    protected DbSet<TEntity> DbSet { get; }

    public EFCoreRepository(DbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = dbContext.Set<TEntity>() ?? throw new ArgumentNullException(nameof(TEntity));
    }

    public virtual Task<TEntity?> Get(TGuid id) => throw new NotImplementedException("Implementation should be inside derived specific class");
    
    public virtual Task<IEnumerable<TEntity>> GetAll(string tagName, string searchTerm) => throw new NotImplementedException("Implementation should be inside derived specific class");
    //------------------------------------------------------

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

    protected async Task<IEnumerable<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderyBy = null,
        Expression<Func<TEntity, object>> include = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = DbSet;
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }
        if (include is not null)
        {
            query = query.Include(include);
        }
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        return orderyBy is null
            ? await query.ToListAsync()
            : await orderyBy(query).ToListAsync();
    }

    public void Remove(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    protected Task<TEntity?> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderyBy = null,
        Expression<Func<TEntity, object>> include = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = DbSet;
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }
        if (include is not null)
        {
            query = query.Include(include);
        }
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        return orderyBy is null
            ? query.FirstOrDefaultAsync()
            : orderyBy(query).FirstOrDefaultAsync();

    }

    
}