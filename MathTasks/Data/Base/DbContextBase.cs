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
            var providers = new Tuple<IEmailProvider, IDateTimeProvider>(new DefaultEmailProvider(), new UtcNowDateTimeProvider());
            GetAddedAuditableItems().ToList()
                .ForEach(entry => UpdateAddedItem(entry, providers));
            GetModifiedAuditableItems().ToList()
                .ForEach(entry => UpdateModifiedItem(entry, providers));
        }

        private IEnumerable<EntityEntry> GetAddedAuditableItems()
        {
            var result = ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Added)
                .Where(entry => entry is IAuditable);
            return result;
        }

        private static void UpdateAddedItem(EntityEntry entry, Tuple<IEmailProvider, IDateTimeProvider> providers)
        {
            var createdBy = entry.Property(nameof(IAuditable.CreatedBy));
            var updatedBy = entry.Property(nameof(IAuditable.UpdatedBy));
            var createdAt = entry.Property(nameof(IAuditable.CreatedAt));
            var updatedAt = entry.Property(nameof(IAuditable.UpdatedAt));

            if (string.IsNullOrEmpty(createdBy?.ToString()))
            {
                entry.Property(nameof(IAuditable.CreatedBy)).CurrentValue = providers.Item1.ToString();
            }

            if (string.IsNullOrEmpty(updatedBy?.ToString()))
            {
                entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = providers.Item1.ToString();
            }

            if (DateTime.Parse(createdAt?.ToString()!).Year < 1970)
            {
                entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = providers.Item2.ToDateTime();
            }

            if ((updatedAt is not null) && DateTime.Parse(updatedAt?.ToString()).Year < 1970)
            {
                entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = providers.Item2.ToDateTime();
            }
            else
            {
                entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = providers.Item2.ToDateTime();

            }
        }

        private IEnumerable<EntityEntry> GetModifiedAuditableItems()
        {
            var result = ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Modified)
                .Where(entry => entry is IAuditable);
            return result;
        }

        private static void UpdateModifiedItem(EntityEntry entry, Tuple<IEmailProvider, IDateTimeProvider> providers)
        {
            var userName = entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue ?? providers.Item1.ToString();
            entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = providers.Item2.ToDateTime();
            entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = userName;
        }
    }
}
