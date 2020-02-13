using Azure.TestProject.Common;
using Azure.TestProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Data
{
    public abstract class DbContextWrapper<TDbContext> where TDbContext : DbContext
    {
        protected DbContextWrapper(TDbContext context)
        {
            Context = context;

            ExceptionHandlersByType = new Dictionary<Type, ExceptionHandler>()
            {
                [typeof(DbEntityValidationException)] = new DbEntityValidationExceptionHandler()
            };
        }

        protected TDbContext Context { get; set; }

        protected IDictionary<Type, ExceptionHandler> ExceptionHandlersByType { get; }

        public virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                SetTimestamps();

                await InternalSaveChangesAsync();

                int affectedRows = await Context.SaveChangesAsync();
                return affectedRows;
            }
            catch (Exception ex)
            {
                if (ExceptionHandlersByType.TryGetValue(ex.GetType(), out ExceptionHandler handler))
                {
                    AZTException oloEx = handler.CreateAZTException(ex);
                    throw oloEx;
                }

                throw;
            }
        }

        protected virtual Task InternalSaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        private void SetTimestamps()
        {
            var dbEntityEntries =
                Context
                    .ChangeTracker
                    .Entries()
                    .Where(
                        entry =>
                        entry.Entity is Auditable
                        &&
                        entry.State == EntityState.Added || entry.State == EntityState.Modified
                    )
                    .ToList();

            if (dbEntityEntries.IsNullOrEmpty())
            {
                return;
            }

            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            foreach (DbEntityEntry dbEntityEntry in dbEntityEntries)
            {
                var auditable = (Auditable)dbEntityEntry.Entity;

                if (dbEntityEntry.State == EntityState.Added)
                {
                    auditable.CreatedAtUtc = utcNow;
                }

                // TODO later we can add...
                //auditable.LastModifiedAtUtc = utcNow;
            }
        }
    }
}
