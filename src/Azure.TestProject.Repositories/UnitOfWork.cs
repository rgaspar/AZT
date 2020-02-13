using Azure.TestProject.Data;
using Azure.TestProject.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Repositories
{
    public class UnitOfWork<TDbContext> : DbContextWrapper<TDbContext>, IUnitOfWork
        where TDbContext : DbContext
    {
        public UnitOfWork(TDbContext context)
            : base(context)
        {
        }
    }
}
