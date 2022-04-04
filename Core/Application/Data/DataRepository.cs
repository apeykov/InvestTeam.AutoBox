namespace InvestTeam.AutoBox.Application.Data
{
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using InvestTeam.AutoBox.Domain.Common;
    using InvestTeam.AutoBox.Domain.Exceptions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class DataRepository<TEntity> : IDataRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IApplicationDbContext context;
        protected readonly DbSet<TEntity> internalEntities;

        public DataRepository(IApplicationDbContext ctx)
        {
            context = ctx;
            internalEntities = context.Set<TEntity>();
        }

        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> filter)
        {
            return internalEntities.SingleOrDefault(filter);
        }

        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await internalEntities.SingleOrDefaultAsync(filter);
        }

        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] selectors)
        {
            IQueryable<TEntity> query = internalEntities;

            foreach (var selector in selectors)
            {
                query = query.Include(selector);
            }

            return query.SingleOrDefault(filter);
        }

        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] selectors)
        {
            IQueryable<TEntity> query = internalEntities;

            foreach (var selector in selectors)
            {
                query = query.Include(selector);
            }

            return await query.SingleOrDefaultAsync(filter);
        }

        public virtual TEntity FindOne(object inMemoryId)
        {
            return internalEntities.Find(inMemoryId);
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] selectors)
        {
            IQueryable<TEntity> query = internalEntities.Where(filter);

            foreach (var selector in selectors)
            {
                query = query.Include(selector);
            }

            return query;
        }

        public virtual IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] selectors)
        {
            return FindBy(entity => true, selectors).ToList();
        }

        public async virtual Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] selectors)
        {
            return await FindBy(entity => true, selectors).ToListAsync();
        }

        public virtual void Add(params TEntity[] entities)
        {
            if (entities == null)
            {
                throw new NullEntityDetectedException(typeof(TEntity));
            }

            internalEntities.AddRange(entities);
        }

        public virtual void Set(params TEntity[] entities)
        {
            ValidateInput(entities);

            internalEntities.UpdateRange(entities);
        }

        public virtual void Remove(params TEntity[] entities)
        {
            ValidateInput(entities);

            internalEntities.RemoveRange(entities);
        }

        private void ValidateInput(TEntity[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("Entities collection passed to repository is null.");
            }

            foreach (var item in entities)
            {
                var keys = internalEntities.EntityType.FindPrimaryKey().Properties.Select(property => property.Name).ToArray();

                var keyvalues = keys
                    .Select(propertyName => item.GetType().GetProperty(propertyName).GetValue(item, null))
                    .ToArray();

                if (internalEntities.Find(keyvalues) == null)
                    throw new EntityNotFoundException(item);

                //if (internalEntities.Find(item.Id) == null)
                //    throw new EntityNotFoundException(item);
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
