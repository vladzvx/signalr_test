using IASK.Cases.AnatomicAtlas;
using IASK.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Tests.Cases.DIComplitionTest
{
    public class AtlasStartup :StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            AnatomicAtlas.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }

    [TestClass]
    public class AnatomicAtlasTest: BaseTest<AtlasStartup>
    {
        public override IEnumerable<string> Apis => new List<string> { "/atlas/getsymptoms" };

        [ClassInitialize]
        public static new void Init(TestContext context)
        {
            BaseTest<AtlasStartup>.Init(context);
        }
    }
}
