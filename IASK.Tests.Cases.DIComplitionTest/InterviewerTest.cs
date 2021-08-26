using IASK.Cases.AnatomicAtlas;
using IASK.Cases.Checker;
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
    public class InterviewerStartup :StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            Checker.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }

    [TestClass]
    public class InterviewerTest : BaseTest<InterviewerStartup>
    {
        public override IEnumerable<string> Apis =>new List<string>() { "/interviewer" } ;

        [ClassInitialize]
        public static new void Init(TestContext context)
        {
            BaseTest<InterviewerStartup>.Init(context);
        }
    }
}
