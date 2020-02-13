using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common
{
    public class ExceptionMessageCollection
    {
        [JsonConstructor]
        public ExceptionMessageCollection() : this(null)
        {
        }

        private ExceptionMessageCollection(IEnumerable<string> messages)
        {
            Messages = messages?.ToArray() ?? new string[] { };
        }

        public string[] Messages { get; set; }

        public static ExceptionMessageCollection Create(Exception ex)
        {
            var messages = GetExceptionMessages(ex).ToList();

            return new ExceptionMessageCollection(messages);
        }

        public static ExceptionMessageCollection Create(string message)
        {
            return Create(new AZTException(message));
        }

        public static ExceptionMessageCollection Create(params string[] messages)
        {
            return new ExceptionMessageCollection(messages);
        }

        public static implicit operator AZTException(ExceptionMessageCollection collection)
        {
            string[] messages = collection.Messages.Reverse().ToArray();

            AZTException currentException = null;

            foreach (string message in messages)
            {
                if (currentException is null)
                {
                    currentException = new AZTException(message);
                }
                else
                {
                    currentException = new AZTException(message, currentException);
                }
            }

            return currentException;
        }

        private static IEnumerable<string> GetExceptionMessages(Exception ex)
        {
            Exception currentException = ex;

            var messages = new List<string>();

            while (currentException != null)
            {
                messages.Add(currentException.Message);

                if (currentException is ReflectionTypeLoadException reflectionTypeLoadException)
                {
                    messages.AddRange(GetExceptionMessages(reflectionTypeLoadException.LoaderExceptions));
                }

                currentException = currentException.InnerException;
            }

            return messages;
        }

        private static IEnumerable<string> GetExceptionMessages(Exception[] exceptions)
        {
            var messages = new List<string>();

            foreach (Exception exception in exceptions)
            {
                messages.AddRange(GetExceptionMessages(exception));
            }

            return messages;
        }
    }
}
