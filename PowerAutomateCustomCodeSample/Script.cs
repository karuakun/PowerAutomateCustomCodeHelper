using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using PowerAutomateCustomCodeHelper;

namespace PowerAutomateCustomCodeSample
{
    public class Script: ScriptBase 
    {
        public override async Task<HttpResponseMessage> ExecuteAsync()
        {
            await Task.CompletedTask;
            return Context.OperationId switch
            {
                "ComputeMD5" => GetComputeMd5(),
                //"SendData" => await PostSendDataAsync(),
                _ => new HttpResponseMessage(HttpStatusCode.BadRequest)
            };
        }

        private HttpResponseMessage GetComputeMd5()
        {
            // コンテキストからクエリ文字列 value を参照する
            var queryString = Context.Request.RequestUri?.Query;
            if (string.IsNullOrEmpty(queryString))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var query = HttpUtility.ParseQueryString(Context.Request.RequestUri.Query);
            var origin = query.AllKeys.Any(_ => _ == "value") 
                ? string.IsNullOrEmpty(query["value"]) ? string.Empty: query["value"]
                : string.Empty;

            // MD5 ハッシュを計算しレスポンスに格納する
            var hashedString = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(origin));
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent((new JObject
            {
                ["origin"] = origin,
                ["hashedString"] = hashedString,
            }).ToString());
            return response;
        }

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        //private async Task<HttpResponseMessage> PostSendDataAsync()
        //{
        //    // Postされたフォームデータから search を参照する
        //    if (Context.Request.Content == null)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    }
        //    var postData = await Context.Request.Content.ReadFromJsonAsync<IPostSendDataAsyncRequest>(_jsonSerializerOptions);
        //    if (string.IsNullOrEmpty(postData?.Search))
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    }

        //    // 外部に通信してデータを取得してレスンポスに格納する
        //    var response = await Context.SendAsync(new HttpRequestMessage
        //    {

        //    }, CancellationToken).ConfigureAwait(false);

        //}

        //public interface IPostSendDataAsyncRequest
        //{
        //    string Search { get; set; }
        //}
    }
}