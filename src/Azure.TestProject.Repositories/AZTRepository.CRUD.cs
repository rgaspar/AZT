using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Repositories
{
    public abstract partial class AZTRepository<TEntity, TTarget, TPrimaryKey>
    {
        protected const bool DefaultActiveOnlyFiltering = false;

        protected virtual Task<int> InternalCreateAsync(DbSet<TEntity> entities, TTarget itemToBeCreated)
        {
            try
            {
                if (itemToBeCreated != null)
                {
                    TEntity entity = Mapper.Map<TEntity>(itemToBeCreated);

                    entities.Add(entity);
                    Context.SaveChangesAsync();
                }

                return Task.FromResult(0);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual async Task<IEnumerable<TTarget>> InternalGetAllAsync(bool activeOnly = DefaultActiveOnlyFiltering)
        {
            IQueryable<TEntity> entityQuery = GetEntities(includeNavigationProperties: true);

            IEnumerable<TEntity> entities = await entityQuery.ToListAsync();

            IEnumerable<TTarget> targets = Mapper.Map<IEnumerable<TTarget>>(entities);
            return targets;
        }

        protected async Task<TTarget> InternalGetAsync(TPrimaryKey primaryKey, Expression<Func<TEntity, bool>> idFilter, bool activeOnly = DefaultActiveOnlyFiltering)
        {
            return await InternalGetAsync("Id", primaryKey, idFilter, activeOnly);
        }

        protected virtual async Task<TTarget> InternalGetAsync(string dataDisplayName, object data, Expression<Func<TEntity, bool>> idFilter, bool activeOnly = DefaultActiveOnlyFiltering)
        {
            IQueryable<TEntity> entityQuery = GetEntities(includeNavigationProperties: true);

            TEntity entity = await entityQuery.SingleOrDefaultAsync(idFilter);

            TTarget target = Mapper.Map<TTarget>(entity);
            return target;
        }
    }
}
