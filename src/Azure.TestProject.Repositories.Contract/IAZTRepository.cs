using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Repositories.Contract
{
    public interface IAZTRepository<TTarget, TPrimaryKey>
        where TTarget : class
    {
        Task<IEnumerable<TTarget>> GetAllAsync();

        Task<TTarget> GetAsync(TPrimaryKey primaryKey);
    }
}
