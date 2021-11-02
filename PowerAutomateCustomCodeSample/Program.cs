using System;
using System.Threading.Tasks;
using PowerAutomateCustomCodeHelper;

namespace PowerAutomateCustomCodeSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var script = TestScriptHelper.CreateGetRequest<Script>("https://api.contoso.com/MD5?value=hoge", "ComputeMD5");
            var response = await script.ExecuteAsync();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
