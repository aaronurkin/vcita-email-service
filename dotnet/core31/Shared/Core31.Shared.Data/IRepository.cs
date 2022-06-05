using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core31.Shared.Data
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        TEntity Insert(TEntity entity);
        TEntity Find(TKey id);
        TEntity Update(TKey id, TEntity entity);
        void Delete(TKey id);
        IEnumerable<TEntity> Select();
        IEnumerable<TEntity> Select(Expression<Func<TEntity, bool>> predicate = null);
    }

    public interface IRepository<TEntity> : IRepository<TEntity, Guid>
        where TEntity : class
    {
    }
}
