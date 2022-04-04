namespace InvestTeam.AutoBox.Application.Common.Interfaces
{
    using InvestTeam.AutoBox.Domain.Common;
    using System;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void RollbackTransaction();

        void CommitTransaction();

        int Done();

        Task<int> DoneAsync();

        IDataRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Reset the dbcontext as discard all changes into its internal entities
        /// </summary>
        void Clear();
    }
}
