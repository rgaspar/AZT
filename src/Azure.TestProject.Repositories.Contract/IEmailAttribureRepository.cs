using Azure.Test.Project.Domain.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Repositories.Contract
{
    public interface IEmailAttribureRepository: IAZTRepository<EmailAttribute, int>, ICanCreate<EmailAttribute>
    {
        Task<IEnumerable<EmailAttribute>> GetAllByByEmailAsync(string email);
    }
}
