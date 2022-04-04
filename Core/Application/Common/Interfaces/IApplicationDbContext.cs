namespace InvestTeam.AutoBox.Application.Common.Interfaces
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IApplicationDbContext : IDisposable
    {
        DatabaseFacade Database { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        void Clear();
    }
}
