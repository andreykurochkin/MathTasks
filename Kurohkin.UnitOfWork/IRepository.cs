﻿using System.Linq.Expressions;

namespace Kurochkin.Persistence.UnitOfWork;

public interface IRepository<TEntity, TGuid>
    where TEntity : class
    where TGuid : struct
{
    // Create
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    // Read
    Task<TEntity?> Get(TGuid id);
    Task<IEnumerable<TEntity>> GetAll(string tagName, string searchTerm);
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> prediate);

    // Delete
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}