using MathTasks.Models.Base;
using MathTasks.Providers;
using MathTasks.Providers.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Collections.Generic;
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
            ProcessAddedEntries(GetAddedAuditableEntries(), utcNowDateTimeProvider, defaultEmailProvider);
            ProcessModifiedEntries(GetModifiedAuditableEntries(), defaultEmailProvider, utcNowDateTimeProvider);
        }

        private IEnumerable<EntityEntry> GetAddedAuditableEntries()
        {
            var result = ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Added)
                .Where(entry => entry is IAuditable);
            return result;
        }

        private void ProcessAddedEntries(IEnumerable<EntityEntry> inputEntries, IDateTimeProvider dateTimeProvider, IEmailProvider emailProvider)
        {
            var entries = inputEntries.ToList();
            entries.ForEach(entry =>
            {
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
            });
        }

        private IEnumerable<EntityEntry> GetModifiedAuditableEntries()
        {
            var result = ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Modified)
                .Where(entry => entry is IAuditable);
            return result;
        }

        private void ProcessModifiedEntries(IEnumerable<EntityEntry> inputEntries, IEmailProvider emailProvider, IDateTimeProvider dateTimeProvider)
        {
            var entries = inputEntries.ToList();
            entries.ForEach(entry =>
                {
                    var userName = entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue ?? emailProvider.ToString();
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = dateTimeProvider.ToDateTime();
                    entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = emailProvider.ToString();
                });
        }
    }
}
