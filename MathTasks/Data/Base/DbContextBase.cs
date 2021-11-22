using MathTasks.Models.Base;
using MathTasks.Providers;
using MathTasks.Providers.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Data
{
    public abstract class DbContextBase : IdentityDbContext
    {
        protected DbContextBase(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(Startup).Assembly);
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            DbSaveChanges();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            DbSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            DbSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DbSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void DbSaveChanges()
        {
            IEmailProvider defaultEmailProvider = new DefaultEmailProvider();
            IDateTimeProvider utcNowDateTimeProvider = new UtcNowDateTimeProvider(); 
            ProcessAddedEntries(defaultDate, defaultEmailProvider);
            ProcessModifiedEntries(defaultEmailProvider);
        }

        private void ProcessAddedEntries(IDateTimeProvider dateTimeProvider, IEmailProvider emailProvider)
        {
            var addedEntities = ChangeTracker.Entries()
                            .Where(entry => entry.State == EntityState.Added);

            foreach (var entry in addedEntities)
            {
                if (entry is not IAuditable)
                {
                    continue;
                }
                var createdBy = entry.Property(nameof(IAuditable.CreatedBy));
                var updatedBy = entry.Property(nameof(IAuditable.UpdatedBy));
                var createdAt = entry.Property(nameof(IAuditable.CreatedAt));
                var updatedAt = entry.Property(nameof(IAuditable.UpdatedAt));

                if (string.IsNullOrEmpty(createdBy?.ToString()))
                {
                    entry.Property(nameof(IAuditable.CreatedBy)).CurrentValue = emailProvider.ToString();
                }

                if (string.IsNullOrEmpty(updatedBy?.ToString()))
                {
                    entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = emailProvider.ToString();
                }

                if (DateTime.Parse(createdAt?.ToString()!).Year < 1970)
                {
                    entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = dateTimeProvider.ToDateTime();
                }

                if ((updatedAt is not null) && DateTime.Parse(updatedAt?.ToString()).Year < 1970)
                {
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = dateTimeProvider.ToDateTime();
                }
            }
        }

        private void ProcessModifiedEntries(IEmailProvider  emailProvider)
        {
            var modifiedEntities = ChangeTracker.Entries()
                            .Where(entry => entry.State == EntityState.Modified);
            foreach (var entry in modifiedEntities)
            {
                if (entry is IAuditable)
                {
                    var userName = entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue ?? emailProvider.ToString();
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = DateTime.UtcNow;
                    entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = emailProvider.ToString();
                }
            }
        }
    }
}
