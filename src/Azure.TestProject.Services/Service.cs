using AutoMapper;
using Azure.TestProject.Common;
using Azure.TestProject.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Services
{
    public abstract class Service
    {
        protected Service(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        protected IUnitOfWork UnitOfWork { get; }

        protected IMapper Mapper { get; }

        protected static void ThrowIfNull<T>(T data, string paramName, string message)
            where T : class
        {
            if (data is null)
            {
                throw new AZTException(
                    message,
                    new ArgumentNullException(paramName),
                    HttpStatusCode.BadRequest
                );
            }
        }

        protected static void ThrowIfInvalidId(int id, string paramName, string message)
        {
            if (id < 1)
            {
                throw new AZTException(
                    message,
                    new ArgumentException($"The specified identifier ({id}) is invalid.", paramName),
                    HttpStatusCode.BadRequest
                );
            }
        }

        protected void ThrowNotFoundIfNull<T>(T model, int modelId)
            where T : class
        {
            ThrowNotFoundIfNull(model, "Id", modelId);
        }

        protected void ThrowNotFoundIfNull<T>(T model, string dataDisplayName, object data)
        where T : class
        {
            if (model is null)
            {
                throw new AZTException($"The requested data ({typeof(T).Name}) could not be found. {dataDisplayName}=[{data}]", HttpStatusCode.NotFound);
            }
        }
    }
}
