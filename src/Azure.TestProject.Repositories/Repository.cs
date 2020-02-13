using AutoMapper;
using Azure.TestProject.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Repositories
{
    public abstract class Repository<TDbContext> : DbContextWrapper<TDbContext>
        where TDbContext : DbContext
    {
        protected Repository(TDbContext context, IMapper mapper)
            : base(context)
        {
            Mapper = mapper;
        }

        protected IMapper Mapper { get; }
    }
}
