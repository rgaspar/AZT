using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Azure.TestProject.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Azure.TestProject.Net.Http.Results
{
    public class AZTWebApiResponse
    {
        [JsonConstructor]
        public AZTWebApiResponse()
        {
        }

        private AZTWebApiResponse(HttpStatusCode httpStatusCode, object content, Type contentClrType)
        {
            Content = content;
            ContentClrType = contentClrType;
            ResponseCode = httpStatusCode;

            int statusCode = (int)httpStatusCode;

            IsSuccess = statusCode >= 200 && statusCode <= 299;
        }

        /// <summary>
        /// The actual HTTP status code.
        /// </summary>
        [Required]
        public HttpStatusCode ResponseCode { get; set; }

        /// <summary>
        /// The actual response.
        /// </summary>
        [Required]
        public object Content { get; set; }

        /// <summary>
        /// The .NET type of the actual response.
        /// </summary>
        [Required]
        public Type ContentClrType { get; set; }

        /// <summary>
        /// Indicates whether the requested operation was successful.
        /// </summary>
        [Required]
        public bool IsSuccess { get; set; }

        public static AZTWebApiResponse Create<T>(HttpStatusCode httpStatusCode, T content)
        {
            object obj = content;
            Type contentClrType = obj?.GetType() ?? typeof(T);

            return new AZTWebApiResponse(httpStatusCode, content, contentClrType);
        }

        public TResponse GetContent<TResponse>()
        {
            if (IsSuccess)
            {
                Type requestedClrType = typeof(TResponse);

                if (IsValidRequestedType(requestedClrType, ContentClrType))
                {
                    TResponse response;

                    switch (Content)
                    {
                        case JToken content:
                            object responseObject = ConvertJTokenToObject(content, ContentClrType);
                            response = Cast<TResponse>(responseObject);
                            break;

                        default:
                            throw new AZTException($"Unknown deserialized content type: [{Content.GetType().FullName}]");
                    }

                    return response;
                }
                else
                {
                    throw new AZTException($"Serialized type does not match requested type. Serialized=[{ContentClrType.FullName}] Requested=[{requestedClrType.FullName}]");
                }
            }

            throw new AZTException($"The {nameof(GetContent)}<TResponse>() method can only be called when {nameof(IsSuccess)} is {Boolean.TrueString}.");
        }

        public ExceptionMessageCollection GetExceptionMessages()
        {
            if (!IsSuccess && HasExceptionMessages())
            {
                ExceptionMessageCollection response = default;

                switch (Content)
                {
                    case ExceptionMessageCollection collection:
                        response = collection;
                        break;

                    case JToken content:
                        object responseObject = ConvertJTokenToObject(content, ContentClrType);
                        response = Cast<ExceptionMessageCollection>(responseObject);
                        break;

                    default:
                        throw new AZTException($"Unknown deserialized content type: [{Content.GetType().FullName}]");
                }

                return response;
            }

            throw new AZTException($"The {nameof(GetExceptionMessages)}() method can only be called when {nameof(IsSuccess)} is {Boolean.FalseString}.");
        }

        public bool HasExceptionMessages()
        {
            return typeof(ExceptionMessageCollection) == ContentClrType;
        }

        private static bool IsValidRequestedType(Type requestedType, Type serializedType)
        {
            bool result =
                requestedType.IsIEnumerable()
                    ? requestedType.IsAssignableFrom(serializedType)
                    : requestedType == serializedType;

            return result;
        }

        private static object ConvertJTokenToObject(JToken token, Type contentClrType)
        {
            try
            {
                return token.ToObject(contentClrType);
            }
            catch (Exception ex)
            {
                throw new AZTException("The JToken instance could not be converted to the serialized type.", ex);
            }
        }

        private static TResponse Cast<TResponse>(object obj)
        {
            try
            {
                return (TResponse)obj;
            }
            catch (Exception ex)
            {
                throw new AZTException("The instance of the serialized type could not be cast to the requested type.", ex);
            }
        }
    }
}
