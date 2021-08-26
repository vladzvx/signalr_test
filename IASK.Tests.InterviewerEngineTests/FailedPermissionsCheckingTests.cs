using IASK.Common;
using IASK.InterviewerEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UMKBRequests;
using UMKBRequests.Models.API.Satellite;
using UMKBRequests.Models.API.Semantic;

namespace IASK.InterviewerEngineTests
{
    [TestClass]
    public class FailedPermissionsCheckingTests
    {

        [TestMethod]
        public void GetSemanticTest1()
        {
            RequestSemantic request = new RequestSemantic()
            {
                libid = new List<string>() { "5060086", "7490154" },
                deep = 1,
                key = Secrets.Semantic,
                lib = new List<ushort>() { 86, 154 },
                level = new List<ushort>() { 1937, 1050, 1051, 1850 },
                route = 0
            };
            GetSemanticNewBigId getSemanticNewBigId11 = new GetSemanticNewBigId();
            var q = getSemanticNewBigId11.Get(request);
            Assert.IsTrue(q.alerts.code == 405);
            Assert.IsTrue(CacheForming.TryCheckSemanticAccess(request, out List<ushort> libs, out List<ushort> levels));
            Assert.IsTrue(libs.Count == 1 && libs[0] == 154);
            Assert.IsTrue(levels.Count == 1 && levels[0] == 1850);
        }


        [TestMethod]
        public void GetSemanticTest2()
        {
            RequestSemantic request = new RequestSemantic()
            {
                libid = new List<string>() { "5060086" },
                deep = 1,
                key = Secrets.Semantic,
                lib = new List<ushort>() { 86 },
                level = new List<ushort>() { 1937, 1050, 1051 },
                route = 0
            };
            GetSemanticNewBigId getSemanticNewBigId11 = new GetSemanticNewBigId();
            var q = getSemanticNewBigId11.Get(request);
            Assert.IsTrue(q.alerts.code == 200);
            Assert.IsFalse(CacheForming.TryCheckSemanticAccess(request, out _, out _));
        }
        [TestMethod]
        public void GetDescriptionsTest1()
        {
            RequestSatellite rs = new RequestSatellite()
            {
                permit = new UMKBRequests.Models.API.Codes.Permit()
                {
                    key = Secrets.Descriptions,
                },
                bigids = new List<string>()
                {
                    IdParser.GetBigId(86,506).ToString(),
                    IdParser.GetBigId(86,6408).ToString(),
                }
            };
            GetDescriptionsDB getDescriptionsDB = new GetDescriptionsDB();
            var res1 = getDescriptionsDB.Get(rs);
            Assert.IsTrue(res1.Alert.code == "200");

        }

        [TestMethod]
        public void GetDescriptionsTest2()
        {
            RequestSatellite rs = new RequestSatellite()
            {
                permit = new UMKBRequests.Models.API.Codes.Permit()
                {
                    key = Secrets.Descriptions,
                },
                bigids = new List<string>()
                {
                    IdParser.GetBigId(912,339).ToString(),
                    IdParser.GetBigId(154,749).ToString(),
                    IdParser.GetBigId(86,506).ToString(),
                }
            };
            GetDescriptionsDB getDescriptionsDB = new GetDescriptionsDB();
            var res1 = getDescriptionsDB.Get(rs);
            Assert.IsTrue(res1.Alert.code == "405");
            Assert.IsTrue(CacheForming.TryCheckDescriptionsAccess(rs, out List<ushort> libs));
            Assert.IsTrue(libs.Count != 0 && libs[0] == 154);
        }

        [TestMethod]
        public void GetVarcharListTest1()
        {
            UMKBRequests.GetVarCharList gvcl = new GetVarCharList();
            RequestVarCharList requestVarCharList = new RequestVarCharList()
            {
                bigids = new List<string>()
                {
                    IdParser.GetBigId(86,506).ToString(),
                    IdParser.GetBigId(86,6408).ToString(),
                },
                permit = new UMKBRequests.Models.API.Codes.Permit()
                {
                    key = Secrets.VarcharList
                },
            };

            var res = gvcl.Get(requestVarCharList);
            Assert.IsNull(res.alerts);
        }

        [TestMethod]
        public void GetVarcharListTest2()
        {
            UMKBRequests.GetVarCharList gvcl = new GetVarCharList();
            RequestVarCharList requestVarCharList = new RequestVarCharList()
            {
                bigids = new List<string>()
                {
                    IdParser.GetBigId(154,749).ToString(),
                    IdParser.GetBigId(86,506).ToString(),
                },
                permit = new UMKBRequests.Models.API.Codes.Permit()
                {
                    key = Secrets.VarcharList
                },
            };

            ResultVarCharList res = gvcl.Get(requestVarCharList);
            Assert.IsTrue(res.alerts != null && res.alerts.code == 500);
            Assert.IsTrue(CacheForming.TryCheckVarcharListAccess(requestVarCharList, out List<ushort> libs));
            Assert.IsTrue(libs.Count != 0 && libs[0] == 154);
        }
    }
}
