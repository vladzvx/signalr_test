using IASK.Common.Models;
using IASK.InterviewerEngine.Models.Input;
using IASK.InterviewerEngine.Models.Output;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using UMKBRequests.Models.API.Codes;

namespace IASK.Tests.InterviewerServiceTests
{
    internal class TestModel : BaseModel
    {

    }
    [TestClass]
    public class InterviewerControllerTests
    {
        static IHost host;
        static readonly Permit permit = new Permit() { authkey = "ssss" };

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            var _host = new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer().UseStartup<InterviewerService.Startup>();
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
            var SendingTask = cl.PostAsync("/Interviewer", httpContent);
            SendingTask.Wait();
            var res = SendingTask.Result;
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
        }

        //[TestMethod]
        //public void DoctorChecking1()
        //{
        //    CheckerInput container = new CheckerInput() { Permit = permit, Lib = 86, Id = 6408 };
        //    CheckerOutput StartePage = SendAnswerAndGetResponse(container, "/Interviewer");
        //    SetAnswer(container, StartePage.TriggerId);

        //    CheckerOutput HighEducation = SendAnswerAndGetResponse(container, "/Interviewer");//ВУЗ
        //    SendAnswerWrapper(HighEducation, container);

        //    CheckerOutput intern = SendAnswerAndGetResponse(container, "/Interviewer");
        //    SendAnswerWrapper(intern, container);

        //    CheckerOutput ordinat = SendAnswerAndGetResponse(container, "/Interviewer");
        //    SendAnswerWrapper(ordinat, container);

        //    CheckerOutput asp = SendAnswerAndGetResponse(container, "/Interviewer");
        //    ReqursiveFinder(asp.Units, InterfaceUnit.UnitType.HEADER, out InterfaceUnit unit);
        //    Assert.IsTrue(unit.Label.Equals("Укажите аспирантуру"));
        //    SendAnswerWrapper(asp, container);

        //    CheckerOutput sert = SendAnswerAndGetResponse(container, "/Interviewer");
        //    ReqursiveFinder(sert.Units, InterfaceUnit.UnitType.HEADER, out unit);
        //    Assert.IsTrue(unit.Label.Equals("Укажите Вашу специальность"));
        //    SendAnswerWrapper(sert, container);

        //    CheckerOutput pract = SendAnswerAndGetResponse(container, "/Interviewer");
        //    SendAnswerWrapper(pract, container);

        //    CheckerOutput science = SendAnswerAndGetResponse(container, "/Interviewer");
        //    SendAnswerWrapper(science, container);

        //    CheckerOutput end = SendAnswerAndGetResponse(container, "/Interviewer");
        //    SendAnswerWrapper(end, container);

        //    end = SendAnswerAndGetResponse(container, "/Interviewer");
        //    SendAnswerWrapper(end, container);

        //    end = SendAnswerAndGetResponse(container, "/Interviewer");
        //    SendAnswerWrapper(end, container);


        //    end = SendAnswerAndGetResponse(container, "/Interviewer");
        //    SendAnswerWrapper(end, container);

        //    SendAnswerAndGetResponse(container, "/Interviewer");

        //}

        public void SendAnswerWrapper(CheckerOutput Interface, CheckerInput container, params InputStruct[] ist)
        {
            SetAnswer(container, Interface.TriggerId, ist);
        }

        private void ReqursiveFinder(List<InterfaceUnit> interfaceUnit, InterfaceUnit.UnitType Type, out InterfaceUnit res)
        {
            res = null;
            foreach (InterfaceUnit iu in interfaceUnit)
            {
                res = ReqursiveFinder2(iu, Type);
                if (res != null)
                    return;
            }
        }

        private InterfaceUnit ReqursiveFinder2(InterfaceUnit interfaceUnit, InterfaceUnit.UnitType Type)
        {
            if (interfaceUnit.Type.Equals(Type))
            {
                return interfaceUnit;
            }
            else if (interfaceUnit.Units != null)
            {
                foreach (InterfaceUnit iu in interfaceUnit.Units)
                {
                    InterfaceUnit iuTemp = ReqursiveFinder2(iu, Type);
                    if (iuTemp != null)
                    {
                        return iuTemp;
                    }
                }
            }
            return null;
        }

        private CheckerOutput SendAnswerAndGetResponse(CheckerInput container, string adress = "/Interviewer")
        {
            var server = host.GetTestServer();
            HttpClient cl = server.CreateClient();
            string content = Newtonsoft.Json.JsonConvert.SerializeObject(container);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, mediaType: "application/json");
            var SendingTask = cl.PostAsync(adress, httpContent);
            SendingTask.Wait();
            var res = SendingTask.Result;
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            var ContentReadingTask = res.Content.ReadAsStringAsync();
            ContentReadingTask.Wait();
            var ResponseContent = ContentReadingTask.Result;
            CheckerOutput output = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckerOutput>(ResponseContent);
            return output;
        }

        private void SetAnswer(CheckerInput AllResponses, string TriggerId, params InputStruct[] inputStructs)
        {
            AllResponses.Answers ??= new List<IASK.InterviewerEngine.Models.Input.Answer>();
            //if (inputStructs!=null&& inputStructs.Length != 0)
            {
                AllResponses.Answers.Add(new IASK.InterviewerEngine.Models.Input.Answer()
                {
                    TriggerId = TriggerId,
                    CheckerResponse = inputStructs
                });
            }
        }
    }
}
