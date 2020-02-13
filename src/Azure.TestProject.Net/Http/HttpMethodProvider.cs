using System.Net.Http;

namespace Azure.TestProject.Net.Http
{
    public static class HttpMethodProvider
    {
        public static HttpMethod Get { get; } = HttpMethod.Get;

        public static HttpMethod Post { get; } = HttpMethod.Post;

        public static HttpMethod Put { get; } = HttpMethod.Put;

        public static HttpMethod Patch { get; } = new HttpMethod("PATCH");

        public static HttpMethod Delete { get; } = HttpMethod.Delete;
    }
}
