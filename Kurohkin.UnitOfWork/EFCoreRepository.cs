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
    protected IEFCoreRepositoryConfigureOptions<TEntity, TGuid>? Options { get; }

    public EFCoreRepository(DbContext dbContext, IEFCoreRepositoryConfigureOptions<TEntity, TGuid>? options = null!)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = dbContext.Set<TEntity>() ?? throw new ArgumentNullException(nameof(TEntity));
        Options = options ?? throw new ArgumentNullException(nameof(TEntity));
    }

    public Task<TEntity?> Get(TGuid id)
    {
        Options!.SearchId = id;  // this is ugly - how to elegant pass that parameter?
        return GetFirstOrDefaultAsync<TEntity>(predicate: Options?.Predicate!,
              orderyBy: Options?.OrderyBy!,
              include: Options?.Include!,
              disableTracking: Options?.DisableTracking ?? true);
    }
    public Task<IEnumerable<TEntity>> GetAll()
    {
        return ToListAsync(predicate: Options?.Predicate!,
              orderyBy: Options?.OrderyBy!,
              include: Options?.Include!,
              disableTracking: Options?.DisableTracking ?? true);
    }
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

    private async Task<IEnumerable<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate = null,
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
            //query = include(query);
            query = include(query);
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

    private Task<TEntity?> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
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

    
}