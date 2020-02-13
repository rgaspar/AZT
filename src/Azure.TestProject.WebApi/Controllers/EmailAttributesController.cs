using Azure.TestProject.DataTransfer;
using Azure.TestProject.DataTransfer.Core;
using Azure.TestProject.Net.Http.Endpoints;
using Azure.TestProject.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Azure.TestProject.WebApi.Controllers
{
    public class EmailAttributesController : AZTController
    {
        private readonly IEmailAttributesService emailAttributesService;

        public EmailAttributesController(IEmailAttributesService emailAttributesService)
        {
            this.emailAttributesService = emailAttributesService;
        }

        [HttpGet]
        [Route(WebApiEndpoints.AZT.EmailAttributes.GetStatus)]
        public string GetStatus()
        {
            return "Service is running!";
        }

        [HttpPost]
        [Route(WebApiEndpoints.AZT.EmailAttributes.Prefix)]
        public async Task<IHttpActionResult> CreateAsync(EmailAttribute emailAttribute)
        {
            await emailAttributesService.DoExecution(emailAttribute);
            return Success(emailAttribute);
        }        
    }
}
