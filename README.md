# PowerAutomateCustomCodeHelper

PowerAutomate �̃J�X�^���R�[�h�쐬���x�����邽�߂̃��C�u�����ł��B

# �܂܂��@�\

- ScriptBase �y�� IScriptContext �̎���

# �g����

�v���W�F�N�g�̍쐬�ƃ��C�u�����̎Q��

```
dotnet new console
dotnet add package PowerAutomateCustomCodeHelper
```

Script.cs �̍쐬

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
        // �R���e�L�X�g����N�G�������� value ���Q�Ƃ���
        var queryString = Context.Request.RequestUri?.Query;
        if (string.IsNullOrEmpty(queryString))
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
        var query = HttpUtility.ParseQueryString(Context.Request.RequestUri.Query);
        var origin = query.AllKeys.Any(_ => _ == "value")
            ? string.IsNullOrEmpty(query["value"]) ? string.Empty : query["value"]
            : string.Empty;

        // MD5 �n�b�V�����v�Z�����X�|���X�Ɋi�[����
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

Program.cs�̕ύX

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

# �Q�l�ɂ����y�[�W����у��|�W�g��

- https://docs.microsoft.com/en-us/connectors/custom-connectors/write-code
- https://rambosan.hatenablog.com/entry/2021/08/17/235149