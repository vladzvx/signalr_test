using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IASK.Cases.JsonStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace IASK.Tests.Cases.JsonStorage
{
    public class JsonStorageStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            IASK.Cases.JsonStorage.JsonStorage.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }

    [TestClass]
    public class JsonStorageTests : BaseTest<JsonStorageStartup>
    {
        public override IEnumerable<string> Apis => new List<string>() { "/JsonStorage/write", "/JsonStorage/read" };

        [ClassInitialize]
        public static new void  Init(TestContext context)
        {
            BaseTest<JsonStorageStartup>.Init(context);
        }
    }
}
