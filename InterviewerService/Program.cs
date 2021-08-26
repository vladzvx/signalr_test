using IASK.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("IASK.Tests.InterviewerEngineTests")]
[assembly: InternalsVisibleTo("IASK.Tests.InterviewerServiceTests")]

namespace InterviewerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //UMKBWorker.GetSemantic(IdParser.GetNewBigId(1,173));
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Any, 5000);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
