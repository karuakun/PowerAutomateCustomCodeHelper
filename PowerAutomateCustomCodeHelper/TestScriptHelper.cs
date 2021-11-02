using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PowerAutomateCustomCodeHelper
{
    public class TestScriptHelper
    {
        public static T CreateGetRequest<T>(string uri, string operationId)
            where T: ScriptBase, new()
        {
            return new T
            {
                Context = new ScriptContext
                {
                    OperationId = operationId,
                    Request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(uri)
                    }
                },
            };
        }

        public static T CreatePostRequest<T>(string uri, string operationId, JObject parameter)
            where T: ScriptBase, new()
        {
            return CreateRequest<T>(uri, HttpMethod.Post, operationId, parameter);
        }

        public static T CreateRequest<T>(string uri, HttpMethod method, string operationId, JObject parameter)
            where T: ScriptBase, new()
        {
            return new T
            {
                Context = new ScriptContext
                {
                    OperationId = operationId,
                    Request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(uri),
                        Content = new StringContent(parameter?.ToString() ?? string.Empty, Encoding.UTF8, "application/json")
                    }
                }
            };
        }

    }
}