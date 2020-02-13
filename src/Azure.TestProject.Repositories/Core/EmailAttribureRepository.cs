using AutoMapper;
using Azure.Test.Project.Domain.Models;
using Azure.TestProject.Data;
using Azure.TestProject.Data.Models.Core;
using Azure.TestProject.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DbEmailAttribute = Azure.TestProject.Data.Models.Core.EmailAttribute;
using EmailAttribute = Azure.Test.Project.Domain.Models.Core.EmailAttribute;

namespace Azure.TestProject.Repositories
{
    public class EmailAttribureRepository : AZTRepository<DbEmailAttribute, EmailAttribute, int>, IEmailAttribureRepository
    {
        public EmailAttribureRepository(AZTDataContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public override async Task<IEnumerable<EmailAttribute>> GetAllAsync()
        {
            return await InternalGetAllAsync();
        }

        public async Task<int> CreateAsync(EmailAttribute itemToBeCreated)
        {
            return await InternalCreateAsync(Context.EmailAttributes, itemToBeCreated);
        }

        protected override IQueryable<DbEmailAttribute> GetEntities(bool includeNavigationProperties)
        {
            IQueryable<DbEmailAttribute> entities = Context.EmailAttributes;

            return entities.AsNoTracking();
        }

        public override async Task<EmailAttribute> GetAsync(int primaryKey)
        {
            return await InternalGetAsync(primaryKey, entity => entity.Id == primaryKey);
        }

        public async Task<IEnumerable<EmailAttribute>> GetAllByByEmailAsync(string email)
        {
            List<DbEmailAttribute> emailAttributesByEmail =
                await(
                    from ea in Context.EmailAttributes
                    where ea.Email == email
                    select ea
                )
                .ToListAsync();

            IEnumerable<EmailAttribute> result = Mapper.Map<IEnumerable<EmailAttribute>>(emailAttributesByEmail);
            return result;
        }
    }
}
