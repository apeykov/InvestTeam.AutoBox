namespace InvestTeam.AutoBox.Application.AppServices
{
    using InvestTeam.AutoBox.Application.Common;
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using InvestTeam.AutoBox.Application.Exceptions;
    using InvestTeam.AutoBox.Domain.Common;
    using InvestTeam.AutoBox.Domain.Entities.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public abstract class InfrastructureService<TEntity>
        where TEntity : BaseEntity,
        IIdentifiableEntity, new()
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IDataRepository<TEntity> repository;

        protected InfrastructureService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            repository = unitOfWork.Repository<TEntity>();
        }

        /// <summary>
        /// Base method for C-operations in CRUD operation set.        
        /// </summary>
        /// <param name="entities">Domain object (entity) for creation</param>
        public virtual async Task<OperationResult<TEntity>> AddComponent(params TEntity[] entities)
        {
            var result = new OperationResult<TEntity>(entities);

            foreach (var item in entities)
            {
                var existing = await repository.GetOneAsync(e => e.Identity == item.Identity);

                if (existing != null)
                {
                    var ex = new DuplicateObjectException(existing.GetType(), item.Identity);

                    result.AddException(ex, item);

                    return result;
                }

                repository.Add(entities);
            }

            await DoneAsync(result);

            return result;
        }

        /// <summary>
        /// Base method for U-operations in CRUD operation set.
        /// It is `protected` because requires specialization and DTO specific input data structure.
        /// </summary>
        /// <param name="entityUpdates">Object with the fresh data for the Update operation</param>
        protected virtual async Task<OperationResult<TEntity>> SetComponent(params TEntity[] entityUpdates)
        {
            var result = new OperationResult<TEntity>(entityUpdates);

            foreach (var entity in entityUpdates)
            {
                repository.Set(entity);
            }

            await DoneAsync(result);

            return result;
        }

        /// <summary>
        /// Base method for D-operations in CRUD operations set.
        /// </summary>
        /// <param name="identities">Identity of the Entity being removed</param>
        /// <returns></returns>
        protected virtual async Task<OperationResult<TEntity>> RemoveComponents(params string[] identities)
        {
            var entitiesAffected = new List<TEntity>();

            foreach (var identity in identities)
            {
                var entity = await repository.GetOneAsync(e => e.Identity == identity);

                if (entity == null)
                {
                    return NonExistentFaultyResult(identity);
                }

                repository.Remove(entity);

                entitiesAffected.Add(entity);
            }

            var result = new OperationResult<TEntity>(entitiesAffected);

            await DoneAsync(result);

            return result;
        }

        protected virtual async Task<OperationResult<TEntity>> RemoveComponent(string identity)
        {
            var entity = await repository.GetOneAsync(e => e.Identity == identity);

            if (entity == null)
            {
                return NonExistentFaultyResult(identity);
            }

            repository.Remove(entity);

            var result = new OperationResult<TEntity>(entity);

            await DoneAsync(result);

            return result;
        }

        private OperationResult<TEntity> NonExistentFaultyResult(string _identity)
        {
            var ex = new NonExistentObjectException(typeof(TEntity), _identity);
            var nullObjectEntity = new TEntity() { };
            var errResult = new OperationResult<TEntity>();

            errResult.AddException(ex, nullObjectEntity);

            return errResult;
        }

        private void Done(OperationResult<TEntity> result)
        {
            try
            {
                result.Affected = unitOfWork.Done();
            }
            catch (Exception ex)
            {
                result.AddException(ex, new TEntity { });

                throw;
            }
        }

        private async Task DoneAsync(OperationResult<TEntity> result)
        {
            try
            {
                result.Affected = await unitOfWork.DoneAsync();
            }
            catch (Exception ex)
            {
                result.AddException(ex, new TEntity { });

                throw;
            }
        }

        /// <summary>
        /// Base method for R-operations in CRUD operations set.
        /// </summary>
        /// <param name="filter">Filter for selecting a subset of entites</param>
        /// <param name="references">Navigation properties for eager loading</param>
        /// <returns></returns>
        public virtual QueryOperationResult<TEntity> GetComponents(
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] references)
        {
            unitOfWork.Clear();

            if (filter == null)
            {
                // Get all entitites if no filter is supplied
                filter = anyEntity => true;
            }

            var queryResults = /*await*/ repository.FindBy(filter, references).ToList(); /*.ToListAsync();*/

            return new QueryOperationResult<TEntity>() { Entities = queryResults };
        }

        /// <summary>
        /// Validate the supplied identities for existence and are collision-free.
        /// </summary>
        /// <param name="currentIdentity">current identity for existence</param>
        /// <param name="newIdentity">new identity for any collisions</param>
        /// <param name="selectors">Ref data, which should be loaded from the data store explicitly</param>
        /// <returns></returns>
        protected async virtual Task<IdentityValidationResult<TEntity>>
            ValidateIdentity(string currentIdentity, string newIdentity,
                params Expression<Func<TEntity, object>>[] selectors)
        {
            IDataRepository<TEntity> repository = unitOfWork.Repository<TEntity>();
            TEntity outdatedEntity = await repository.GetOneAsync(e => e.Identity == currentIdentity, selectors);

            if (outdatedEntity == null)
            {
                var ex = new NonExistentObjectException(typeof(TEntity), currentIdentity);

                return new IdentityValidationResult<TEntity>(new TEntity(), ex);
            }

            TEntity existing = await repository.GetOneAsync(e => e.Identity == newIdentity);

            if (existing != null)
            {
                var ex = new DuplicateObjectException(typeof(TEntity), newIdentity);

                return new IdentityValidationResult<TEntity>(outdatedEntity, ex);
            }

            return new IdentityValidationResult<TEntity>(outdatedEntity);
        }

        protected virtual OperationResult<TEntity> BuildFailureResult(Func<OperationResult<TEntity>> builder)
        {
            unitOfWork.Clear();

            return builder.Invoke();
        }
    }
}