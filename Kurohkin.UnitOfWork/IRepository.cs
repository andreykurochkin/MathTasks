using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kurochkin.Persistene.UnitOfWork;

public interface IRepository<TEntity, TGuid>
    where TEntity : class
    where TGuid : struct
{
    // Create
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    // Read
    Task<TEntity?> Get(TGuid id);
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> prediate);

    // Delete
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}
