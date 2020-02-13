using AutoMapper;
using Azure.TestProject.Data;
using Azure.TestProject.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Repositories
{
    public abstract partial class AZTRepository<TEntity, TTarget, TPrimaryKey>
        : Repository<AZTDataContext>, IAZTRepository<TTarget, TPrimaryKey>
        where TEntity : class
        where TTarget : class
    {
        protected AZTRepository(AZTDataContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        protected Type EntityType { get; } = typeof(TEntity);

        protected string EntityDisplayName { get; set; } = typeof(TEntity).Name;

        public abstract Task<IEnumerable<TTarget>> GetAllAsync();

        public abstract Task<TTarget> GetAsync(TPrimaryKey primaryKey);

        protected abstract IQueryable<TEntity> GetEntities(bool includeNavigationProperties);
    }
}
