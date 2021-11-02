using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerAutomateCustomCodeHelper
{
    public abstract class ScriptBase {
        // Context object
        public IScriptContext Context { get; init; }

        // CancellationToken for the execution
        public CancellationToken CancellationToken { get; init; }

        // Helper: Creates a StringContent object from the serialized JSON
        public static StringContent CreateJsonContent(string serializedJson) {
            return new StringContent(serializedJson, Encoding.UTF8, "application/json");
        }

        // Abstract method for your code
        public abstract Task<HttpResponseMessage> ExecuteAsync();
    }
}