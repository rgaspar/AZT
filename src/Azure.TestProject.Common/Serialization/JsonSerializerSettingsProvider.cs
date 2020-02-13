using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common
{
    public static class JsonSerializerSettingsProvider
    {
        private const string DefaultCultureName = "en-US";

        private static readonly DefaultContractResolver defaultContractResolver =
            new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

        public static JsonSerializerSettings Settings { get; } =
            new JsonSerializerSettings()
            {
                ContractResolver = defaultContractResolver,
                Culture = CultureInfo.GetCultureInfo(DefaultCultureName),
                DateParseHandling = DateParseHandling.DateTimeOffset
            };
    }
}
