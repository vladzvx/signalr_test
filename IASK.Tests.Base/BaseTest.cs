using IASK.Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using UMKBRequests.Models.API.Codes;

namespace IASK.Tests
{
    [TestClass]
    public abstract class BaseTest<Startup> where Startup : class
    {
        public virtual IEnumerable<string> Apis => new List<string>();

        public static IHost host;
        public static readonly Permit permit = new Permit() { authkey = "ssss" };

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            var _host = new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer().UseStartup<Startup>();
            });
            host = _host.Build();
            host.RunAsync();
        }

        [TestMethod]
        public void ControllerAvailabilityChecking()
        {
            BaseModel container = new TestModel() { Permit = permit };
            string testContent = Newtonsoft.Json.JsonConvert.SerializeObject(container);
            var server = host.GetTestServer();
            HttpClient cl = server.CreateClient();
            HttpContent httpContent = new StringContent(testContent, Encoding.UTF8, mediaType: "application/json");
            foreach (string Api in Apis)
            {
                var SendingTask = cl.PostAsync(Api, httpContent);
                SendingTask.Wait();
                var res = SendingTask.Result;
                Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            }
        }

        public (HttpStatusCode Code,string Content) CheckApi(string content,string api, string mediaType = "application/json", Encoding encoding =null)
        {
            var server = host.GetTestServer();
            HttpClient cl = server.CreateClient();
            if (encoding == null) encoding = Encoding.UTF8;
            HttpContent httpContent = new StringContent(content, encoding, mediaType: mediaType);
            HttpResponseMessage result = cl.PostAsync(api, httpContent).Result;
            return (result.StatusCode, result.Content.ReadAsStringAsync().Result);
        }
    }
}
