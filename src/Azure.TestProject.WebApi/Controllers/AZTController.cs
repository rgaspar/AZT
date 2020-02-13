using Azure.TestProject.Common;
using Azure.TestProject.Net.Http.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace Azure.TestProject.WebApi.Controllers
{
    public abstract class AZTController : ApiController
    {
        private static readonly object objNull = null;

        protected JsonResult<AZTWebApiResponse> Success()
        {
            return
                Json(
                    AZTWebApiResponse.Create(HttpStatusCode.NoContent, objNull),
                    JsonSerializerSettingsProvider.Settings,
                    EncodingProvider.UTF8WithoutBOM
                );
        }

        protected JsonResult<AZTWebApiResponse> Success<T>(T content)
        {
            return
                Json(
                    AZTWebApiResponse.Create(HttpStatusCode.OK, content),
                    JsonSerializerSettingsProvider.Settings,
                    EncodingProvider.UTF8WithoutBOM
                );
        }
    }
}