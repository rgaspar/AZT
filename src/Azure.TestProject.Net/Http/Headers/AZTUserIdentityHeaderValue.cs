using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

using Azure.TestProject.Common;

using Newtonsoft.Json;

namespace Azure.TestProject.Net.Http.Headers
{
    public class AZTUserIdentityHeaderValue : NameValueHeaderValue
    {
        public AZTUserIdentityHeaderValue(string name, string value)
            : base(name, value)
        {
        }

        public static AZTUserIdentityHeaderValue Create(IReadOnlyDictionary<string, IReadOnlyCollection<string>> identity)
        {
            if (identity is null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            string serializedIdentity = JsonConvert.SerializeObject(identity);
            byte[] identityBytes = Encoding.UTF8.GetBytes(serializedIdentity).ToArray();
            string headerValue = new string(identityBytes.ToHexadecimalString().Reverse().ToArray());

            return new AZTUserIdentityHeaderValue(HttpHeaderName.XAZTUserIdentity, headerValue);
        }
    }
}
