using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azure.TestProject.Net.Http.Endpoints
{
    public class WebApiEndpointBuilder
    {
        private readonly string apiEndpoint;

        private readonly Dictionary<string, List<string>> queryParams = new Dictionary<string, List<string>>();

        public WebApiEndpointBuilder(string apiEndpoint)
        {
            this.apiEndpoint = apiEndpoint.EnsureTrailingSlash();
        }

        public WebApiEndpointBuilder QueryParam(string name, params string[] newValues)
        {
            if (queryParams.TryGetValue(name, out List<string> values))
            {
                values.AddRange(newValues);
            }
            else
            {
                values = new List<string>(newValues);
            }

            queryParams[name] = values;

            return this;
        }

        public override string ToString()
        {
            string result =
                new StringBuilder(apiEndpoint)
                    .Append("?")
                    .Append(
                        String.Join(
                            "&",
                            queryParams.SelectMany(
                                queryParam => queryParam.Value.Select(value => $"{queryParam.Key}={value}")
                            )
                        )
                    )
                    .ToString();

            return result;
        }
    }
}
