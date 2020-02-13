using Azure.TestProject.Common;

namespace Azure.TestProject.Net.Http.Endpoints
{
    public static partial class WebApiEndpoints
    {
        public static class AZT
        {
            public static class EmailAttributes
            {
                public const string Prefix = ApiRoot + UrlPathSeparator + nameof(EmailAttributes);

                public const string GetStatus = Prefix + UrlPathSeparator + "GetStatus";
            }
        }
    }
}