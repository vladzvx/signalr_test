using IASK.Cases.EMCReader;
using IASK.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Tests.Cases.EMCReader
{
    public class EMCReaderStartup :StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            IASK.Cases.EMCReader.EMCReader.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }

    [TestClass]
    public class EMCReaderTest : BaseTest<EMCReaderStartup>
    {
        public override IEnumerable<string> Apis => new List<string>() { "/EMC/getprotocol" };

        [ClassInitialize]
        public static new void Init(TestContext context)
        {
            BaseTest<EMCReaderStartup>.Init(context);
        }
    }
}
