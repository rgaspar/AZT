using Azure.TestProject.DataTransfer.Core;
using System.Threading.Tasks;

namespace Azure.TestProject.Services.Contracts
{
    public interface IEmailAttributesService
    {
        Task DoExecution(EmailAttribute emailAttribute);
    }
}
