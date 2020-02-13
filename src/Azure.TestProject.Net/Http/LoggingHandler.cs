using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.TestProject.Net.Http
{
    public class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();

            await LogRequestInfoAsync(request, sb);

            HttpResponseMessage response;

            try
            {
                response = await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                string requestString = sb.ToString();

                WriteLog(requestString);

                throw new Exception("Exception was thrown for this request:\n\n" + requestString, ex);
            }

            await LogResponseInfoAsync(response, sb);

            WriteLog(sb.ToString());

            return response;
        }

        private async Task LogRequestInfoAsync(HttpRequestMessage httpRequestMessage, StringBuilder sb)
        {
            sb.AppendLine("Request:");
            sb.AppendLine(httpRequestMessage.ToString());

            if (httpRequestMessage.Content != null)
            {
                sb.AppendLine(await httpRequestMessage.Content.ReadAsStringAsync());
            }

            sb.AppendLine();
        }

        private async Task LogResponseInfoAsync(HttpResponseMessage httpResponseMessage, StringBuilder sb)
        {
            sb.AppendLine("Response:");
            sb.AppendLine(httpResponseMessage.ToString());

            if (httpResponseMessage.Content != null)
            {
                sb.AppendLine(await httpResponseMessage.Content.ReadAsStringAsync());
            }

            sb.AppendLine();
        }

        [Conditional("DEBUG")]
        private void WriteLog(string message)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(message);
            }
        }
    }
}
