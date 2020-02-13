using System.Net;
using System.Net.Http;

using Azure.TestProject.Common;
using Azure.TestProject.Net.Mime;

using Newtonsoft.Json;

namespace Azure.TestProject.Net.Http.Results
{
    public static class AZTJsonHttpResponseMessageHelper
    {
        public static HttpResponseMessage Create(object rawContent, HttpRequestMessage httpRequestMessage)
        {
            string jsonContent = JsonConvert.SerializeObject(rawContent, JsonSerializerSettingsProvider.Settings);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonContent, EncodingProvider.UTF8WithoutBOM, MediaTypeNames.Application.Json),
                RequestMessage = httpRequestMessage
            };

            return httpResponseMessage;
        }
    }
}
