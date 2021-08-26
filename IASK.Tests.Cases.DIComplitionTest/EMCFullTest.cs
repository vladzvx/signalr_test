using IASK.Cases.AnatomicAtlas;
using IASK.Cases.Checker;
using IASK.Cases.EMCReader;
using IASK.Cases.EMCWriter;
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
    public class EMCStartup :StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            EMCWriter.ConfigureServices(services);
            EMCReader.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }

    [TestClass]
    public class EMCFullTest : BaseTest<EMCStartup>
    {
        public override IEnumerable<string> Apis => new List<string>() { "/EMC/writeprotocol", "/EMC/getprotocol" };

        [ClassInitialize]
        public static new void Init(TestContext context)
        {
            BaseTest<EMCStartup>.Init(context);
        }
    }
}
