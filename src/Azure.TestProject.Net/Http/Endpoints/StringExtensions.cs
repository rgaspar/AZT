namespace Azure.TestProject.Net.Http.Endpoints
{
    public static class StringExtensions
    {
        public static string EnsureTrailingSlash(this string url)
        {
            return url.EndsWith(WebApiEndpoints.UrlPathSeparator) ? url : url + WebApiEndpoints.UrlPathSeparator;
        }

        public static WebApiEndpointBuilder CreateWebApiEndpointBuilder(this string apiEndpoint)
        {
            return new WebApiEndpointBuilder(apiEndpoint);
        }
    }
}
