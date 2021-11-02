# PowerAutomateCustomCodeHelper

PowerAutomate のカスタムコード作成を支援するためのライブラリです。

# 含まれる機能

- ScriptBase 及び IScriptContext の実装

# 使い方

プロジェクトの作成とライブラリの参照

```
dotnet new console
dotnet add package PowerAutomateCustomCodeHelper
```

Script.cs の作成

```cs:Script.cs
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using PowerAutomateCustomCodeHelper;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        await Task.CompletedTask;
        return Context.OperationId switch
        {
            "ComputeMD5" => GetComputeMd5(),
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
            ? string.IsNullOrEmpty(query["value"]) ? string.Empty : query["value"]
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
}
```

Program.csの変更

```cs:Program.cs
using System;
using System.Threading.Tasks;
using PowerAutomateCustomCodeHelper;

class Program
{
    static async Task Main(string[] args)
    {
        var script = TestScriptHelper.CreateGetRequest<Script>("https://api.contoso.com/MD5?value=hoge", "ComputeMD5");
        var response = await script.ExecuteAsync();
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}

```

# 参考にしたページおよびリポジトリ

- https://docs.microsoft.com/en-us/connectors/custom-connectors/write-code
- https://rambosan.hatenablog.com/entry/2021/08/17/235149