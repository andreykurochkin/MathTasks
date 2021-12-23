using Kurochkin.Persistene.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kurohkin.Persistene.UnitOfWork;

/// <summary>
/// stupid class. got to be renamed to postgresql 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TGuid"></typeparam>
public class RepositoryBase<TEntity, TGuid> : IRepository<TEntity, TGuid> where TEntity : class where TGuid : struct
{
    protected DbContext DbContext { get; }
    protected DbSet<TEntity> DbSet { get; }

    public RepositoryBase(DbContext context)
    {
        DbContext = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = DbContext.Set<TEntity>();
    }

    public void Add(TEntity entity) => DbSet.Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> prediate) => DbContext.Set<TEntity>().Where(prediate);


    public Task<TEntity?> Get(TGuid id) => Task.FromResult(DbSet!.Find(id));

    public IEnumerable<TEntity> GetAll() => DbContext.Set<TEntity>().ToList();

    public void Remove(TEntity entity) => DbContext.Set<TEntity>().Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) => DbContext.Set<TEntity>().RemoveRange(entities);
}