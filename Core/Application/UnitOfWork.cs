namespace InvestTeam.AutoBox.Application
{
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using InvestTeam.AutoBox.Application.Data;
    using InvestTeam.AutoBox.Application.Exceptions;
    using InvestTeam.AutoBox.Domain.Common;
    using Microsoft.EntityFrameworkCore.Storage;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork
    {
        protected readonly IApplicationDbContext dbContext;
        protected readonly IDictionary<Type, object> repositories; // cache
        protected IDbContextTransaction transaction;

        public UnitOfWork(IApplicationDbContext ctx)
        {
            dbContext = ctx;
            repositories = new Dictionary<Type, object>();
        }

        public virtual void BeginTransaction()
        {
            // Check if there is an already opened transaction            
            if (transaction != null)
            {
                // => IF yes, this is interpreted as improper transaction release from previous operation run
                // => Nested transactions per one UnitOfWork instance are not supported                
                transaction.Rollback();

                transaction.Dispose();
                transaction = null;
            }

            transaction = dbContext.Database.BeginTransaction();

        }

        public virtual void RollbackTransaction()
        {
            if (transaction == null)
            {
                throw new RollbackTransactionException();
            }

            transaction.Rollback();

            transaction.Dispose();
            transaction = null;

            dbContext.Clear();
        }

        public virtual void CommitTransaction()
        {
            if (transaction == null)
            {
                throw new CommitTransactionException();
            }

            transaction.Commit();

            transaction.Dispose();
            transaction = null;
        }

        /// <summary>
        /// Synchronous commit of all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>Affected DB rows</returns>
        public virtual int Done()
        {
            return dbContext.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>        
        /// The task result contains the number of state entries written to the underlying database. 
        /// This can include state entries for entities and/or relationships. 
        /// </returns>
        public async virtual Task<int> DoneAsync()
        {
            return await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public virtual IDataRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            Type repoType = typeof(TEntity);

            if (!repositories.ContainsKey(repoType))
            {
                var newRepo = new DataRepository<TEntity>(dbContext);
                repositories.Add(repoType, newRepo);
            }

            return (IDataRepository<TEntity>)repositories[repoType];
        }

        public void Dispose()
        {
            dbContext.Dispose();

            if (transaction != null)
            {
                transaction.Dispose();
            }
        }

        /// <summary>
        /// Reset the db context - discards all the changes into the internal/tracked entities
        /// </summary>
        public void Clear()
        {
            dbContext.Clear();
        }
    }
}