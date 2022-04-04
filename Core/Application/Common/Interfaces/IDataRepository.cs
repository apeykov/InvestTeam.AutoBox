namespace InvestTeam.AutoBox.Application.Common.Interfaces
{
    using InvestTeam.AutoBox.Domain.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IDataRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        TEntity GetOne(Expression<Func<TEntity, bool>> filter);

        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter);

        TEntity GetOne(Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] selectors);

        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] selectors);

        TEntity FindOne(object inMemoryId);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] selectors);

        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] selectors);

        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] selectors);

        void Add(params TEntity[] entities);

        void Set(params TEntity[] entities);

        void Remove(params TEntity[] entities);
    }
}
