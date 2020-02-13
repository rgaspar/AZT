using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.TestProject.Common;
using Azure.TestProject.Net.Mime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Azure.TestProject.Net.Http
{
    public abstract class WebApiClient<TToken>
    {
        private readonly HttpClient httpClient;

        public WebApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        protected async Task<HttpResponseMessage> HttpGetAsync(string apiEndpoint, TToken token = default)
        {
            return await HttpSendAsync(HttpMethodProvider.Get, apiEndpoint, token: token);
        }

        protected async Task<HttpResponseMessage> HttpPostAsync(string apiEndpoint, object data, TToken token = default)
        {
            return await HttpSendAsync(HttpMethodProvider.Post, apiEndpoint, data, token);
        }

        protected async Task<HttpResponseMessage> HttpPutAsync(string apiEndpoint, object data, TToken token = default)
        {
            return await HttpSendAsync(HttpMethodProvider.Put, apiEndpoint, data, token);
        }

        protected async Task<HttpResponseMessage> HttpPatchAsync(string apiEndpoint, object data, TToken token = default)
        {
            return await HttpSendAsync(HttpMethodProvider.Patch, apiEndpoint, data, token);
        }

        protected async Task<HttpResponseMessage> HttpDeleteAsync(string apiEndpoint, TToken token = default)
        {
            return await HttpSendAsync(HttpMethodProvider.Delete, apiEndpoint, token: token);
        }

        private async Task<HttpResponseMessage> HttpSendAsync(
            HttpMethod httpMethod,
            string apiEndpoint,
            object data = null,
            TToken token = default
        )
        {
            HttpRequestMessage httpRequestMessage = CreateHttpRequestMessage(httpMethod, apiEndpoint, data, token);
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            return httpResponseMessage;
        }

        protected async Task<TContent> GetContentAsync<TContent>(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();

                if (CanParseJson(jsonContent))
                {
                    TContent content = JsonConvert.DeserializeObject<TContent>(jsonContent);
                    return content;
                }
            }

            return default;
        }

        protected async Task<List<TContent>> GetContentAsyncList<TContent>(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();

                if (CanParseJson(jsonContent))
                {
                    List<TContent> content = JsonConvert.DeserializeObject<List<TContent>>(jsonContent);
                    return content;
                }
            }

            return default;
        }

        protected abstract void SetAuthorizationHeader(HttpRequestMessage httpRequestMessage, TToken token);

        protected virtual void SetContent(HttpRequestMessage httpRequestMessage, object data)
        {
            if (data is null)
            {
                return;
            }
            var s = JsonConvert.SerializeObject(data, JsonSerializerSettingsProvider.Settings);
            httpRequestMessage.Content =
                new StringContent(
                    s,
                    EncodingProvider.UTF8WithoutBOM,
                    MediaTypeNames.Application.Json
                );
        }

        private HttpRequestMessage CreateHttpRequestMessage(
            HttpMethod httpMethod,
            string apiEndpoint,
            object data,
            TToken token
        )
        {
            apiEndpoint = apiEndpoint.EnsureSingleUrlSeparatorChars();

            var httpRequestMessage = new HttpRequestMessage(httpMethod, new Uri(httpClient.BaseAddress, apiEndpoint));

            SetAuthorizationHeader(httpRequestMessage, token);

            SetContent(httpRequestMessage, data);

            return httpRequestMessage;
        }

        private bool CanParseJson(string jsonContent)
        {
            try
            {
                JToken.Parse(jsonContent);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}