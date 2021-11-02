using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PowerAutomateCustomCodeHelper
{
    public class ScriptContext : IScriptContext
    {
        private static readonly HttpClient Client = new();
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
        public string OperationId { get; set; }
        public HttpRequestMessage Request { get; set; }
        public ILogger Logger { get; set; }

        public ScriptContext()
        {
            Request = new HttpRequestMessage();
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Client.SendAsync(request, cancellationToken);
        }
    }
}