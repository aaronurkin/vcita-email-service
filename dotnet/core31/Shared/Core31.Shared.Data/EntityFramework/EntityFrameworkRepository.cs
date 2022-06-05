using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core31.Shared.Data.EntityFramework
{
    public class EntityFrameworkRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly DbContext context;

        public EntityFrameworkRepository(DbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Delete(TKey id)
        {
            var entity = this.Find(id);

            if (entity != null)
            {
                this.context.Set<TEntity>().Remove(entity);
                this.context.SaveChanges();
            }
        }

        public TEntity Find(TKey id)
        {
            var entity = this.context.Set<TEntity>()
                .SingleOrDefault(e => EqualityComparer<TKey>.Default.Equals(e.Id, id));
            return entity;
        }

        public TEntity Insert(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
            this.context.SaveChanges();

            return entity;
        }

        public IEnumerable<TEntity> Select()
        {
            return this.context.Set<TEntity>().AsEnumerable();
        }

        public IEnumerable<TEntity> Select(Expression<Func<TEntity, bool>> predicate = null)
        {
            return this.context.Set<TEntity>().Where(predicate).AsEnumerable();
        }

        public TEntity Update(TKey id, TEntity entity)
        {
            var stored = this.Find(id);

            if (stored == null)
            {
                return entity;
            }

            this.context.Entry(stored).CurrentValues.SetValues(entity);
            this.context.SaveChanges();

            return stored;
        }
    }

    public class EntityFrameworkRepository<TEntity> : EntityFrameworkRepository<TEntity, Guid>
        where TEntity : class, IEntity<Guid>
    {
        public EntityFrameworkRepository(DbContext context)
            : base(context)
        {
        }
    }
}
