using Autofac.Features.Indexed;
using AutoMapper;
using Azure.TestProject.Repositories.Contract;

namespace Azure.TestProject.Services
{
    public abstract class AZTService : Service
    {
        protected AZTService(IIndex<string, IUnitOfWork> unitsOfWork, IMapper mapper)
            : base(unitsOfWork["Default"], mapper)
        {
        }
    }
}
