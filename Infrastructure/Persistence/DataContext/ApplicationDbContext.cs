namespace InvestTeam.AutoBox.Infrastructure.Persistence
{
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using InvestTeam.AutoBox.Domain.Common;
    using InvestTeam.AutoBox.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService,
            IDateTime dateTime) : this(options)
        {
            this.currentUserService = currentUserService;
            this.dateTime = dateTime;
        }

        /// <summary>
        /// Special-purpose constructor, used by `dotnet ef migrations add` tool 
        /// </summary>        
        internal ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // The flag that follows have not much impact, because in EF Core lazy loading is supported 
            // with additional proxy infrastructure and packages.
            // It is added for explicit pointing out that we currently not using Lazy loading.
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<AppSetting> AppSettings { get; set; }

        public DbSet<Error> Errors { get; set; }

        public DbSet<History> History { get; set; }

        public DbSet<RESTLog> RestLogs { get; set; }

        public DbSet<Vechicle> Vechicles { get; set; }

        public DbSet<Order> Orders { get; set; }

        public override int SaveChanges()
        {
            PreSaveChanges();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            PreSaveChanges();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void PreSaveChanges()
        {
            ValidateDomainEntities();

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = currentUserService.UserId;
                        entry.Entity.Created = dateTime.Now;

                        break;

                    case EntityState.Modified:
                        var oldState = entry.OriginalValues.ToObject();
                        var newState = entry.CurrentValues.ToObject();

                        if (newState.Equals(oldState))
                        {
                            break;
                        }

                        entry.Entity.LastModifiedBy = currentUserService.UserId;
                        entry.Entity.LastModified = dateTime.Now;

                        break;
                }
            }
        }

        private void ValidateDomainEntities()
        {
            if (!Database.IsSqlServer())
            {
                //  This method is not supported for Unit / Integration test (in-memory) contexts.                
                return;
            }

            var persistenceCandidates = ChangeTracker.Entries<IValidatableEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            if (persistenceCandidates.Count() > 0)
            {
                foreach (var entry in persistenceCandidates)
                {
                    var entityValidationResult = entry.Entity.Validate();

                    if (!entityValidationResult.IsValid)
                    {
                        throw new ApplicationException(entityValidationResult.Reason);
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        DbSet<TEntity> IApplicationDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public void Clear()
        {
            base.ChangeTracker.Clear();
        }

    }
}
