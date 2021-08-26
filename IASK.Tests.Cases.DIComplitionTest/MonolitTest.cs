using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IASK.Cases.JsonStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using IASK.Cases.AnatomicAtlas;
using IASK.Cases.Checker;
using IASK.Cases.ETIntegration;
using IASK.Cases.InterviewerControllers;
using IASK.Cases.EMCReader;
using IASK.Cases.EMCWriter;

namespace IASK.Tests.Monolit
{
    public class MonolitStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            AnatomicAtlas.ConfigureServices(services);
            Checker.ConfigureServices(services);
            ET.ConfigureServices(services);
            UI.ConfigureServices(services);
            EMCReader.ConfigureServices(services);
            EMCWriter.ConfigureServices(services);
            JsonStorage.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }

    [TestClass]
    public class MonolitTests : BaseTest<MonolitStartup>
    {
        public override IEnumerable<string> Apis => new List<string>() { "/JsonStorage/write", 
            "/JsonStorage/read", "/EMC/writeprotocol","/EMC/getprotocol","/interviewer" ,"/atlas/getsymptoms" ,"/Interface","/Search" };

        [ClassInitialize]
        public static new void Init(TestContext context)
        {
            BaseTest<MonolitStartup>.Init(context);
        }
    }
}
