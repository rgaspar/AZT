using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Repositories.Contract
{
    public interface ICanCreate<TTarget>
        where TTarget : class
    {
        Task<int> CreateAsync(TTarget itemToBeCreated);
    }
}
