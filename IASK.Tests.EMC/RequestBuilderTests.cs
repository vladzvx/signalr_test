using EMCCore.Models.UniversalFilters;
using IASK.EMC.Core.Services;
using IASK.Tests.Base.Services.DbWorkngMoq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace IASK.Tests.EMC
{

    [TestClass]
    public class RequestBuilderTests
    {
        SQLRequestBuilder builder = new SQLRequestBuilder();
        DbConnectionMoq conn = new DbConnectionMoq();

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {

        }

        [TestMethod]
        public void TestMethod1()
        {
            Filter filter = new Filter();
            filter.PatientIds = new ulong[3] {1,2,3 };
            DbCommand command =  builder.Build(filter, conn);
            Assert.IsTrue(command.CommandText.Equals(SQLRequestBuilder.Consts.RequestStart+" ( patient_id = ANY(@patIds)) "+ SQLRequestBuilder.Consts.Appendix+ ";"));
            Assert.IsTrue(command.Parameters.Contains("patIds"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            Filter filter = new Filter();
            filter.ProtocolsIds = new string[3] { "1", "2", "3" };
            DbCommand command = builder.Build(filter, conn);
            Assert.IsTrue(command.CommandText.Equals(SQLRequestBuilder.Consts.RequestStart + " ( id = ANY(@protoIds)) " + SQLRequestBuilder.Consts.Appendix + ";"));
            Assert.IsTrue(command.Parameters.Contains("protoIds"));
        }

        [TestMethod]
        public void TestMethod3()
        {
            Filter filter = new Filter();
            filter.ProtocolsIds = new string[3] { "1", "2", "3" };
            filter.PatientIds = new ulong[3] { 1, 2, 3 };
            DbCommand command = builder.Build(filter, conn);
            Assert.IsTrue(command.CommandText.Equals(SQLRequestBuilder.Consts.RequestStart + " ( id = ANY(@protoIds) AND patient_id = ANY(@patIds)) " + SQLRequestBuilder.Consts.Appendix + ";"));
            Assert.IsTrue(command.Parameters.Contains("protoIds"));
            Assert.IsTrue(command.Parameters.Contains("patIds"));
        }

        [TestMethod]
        public void TestMethod4()
        {
            Filter filter = new Filter();
            filter.From = new DateTime(2010, 1, 1);
            filter.ProtocolsIds = new string[3] { "1", "2", "3" };
            filter.PatientIds = new ulong[3] { 1, 2, 3 };
            DbCommand command = builder.Build(filter, conn);
            Assert.IsTrue(command.CommandText.Equals(SQLRequestBuilder.Consts.RequestStart + " creation_timestamp >= @fromDate and ( id = ANY(@protoIds) AND patient_id = ANY(@patIds)) " + SQLRequestBuilder.Consts.Appendix + ";"));
            Assert.IsTrue(command.Parameters.Contains("protoIds"));
            Assert.IsTrue(command.Parameters.Contains("patIds"));
            Assert.IsTrue(command.Parameters.Contains("fromDate"));
        }

        [TestMethod]
        public void TestMethod5()
        {
            Filter filter = new Filter();
            filter.To = new DateTime(2010, 1, 1);
            filter.ProtocolsIds = new string[3] { "1", "2", "3" };
            filter.PatientIds = new ulong[3] { 1, 2, 3 };
            DbCommand command = builder.Build(filter, conn);
            Assert.IsTrue(command.CommandText.Equals(SQLRequestBuilder.Consts.RequestStart + " creation_timestamp <= @toDate and ( id = ANY(@protoIds) AND patient_id = ANY(@patIds)) " + SQLRequestBuilder.Consts.Appendix + ";"));
            Assert.IsTrue(command.Parameters.Contains("protoIds"));
            Assert.IsTrue(command.Parameters.Contains("patIds"));
            Assert.IsTrue(command.Parameters.Contains("toDate"));
        }

        [TestMethod]
        public void TestMethod6()
        {
            Filter filter = new Filter();
            filter.From = new DateTime(2010, 1, 1);
            filter.To = new DateTime(2010, 2, 1);
            filter.ProtocolsIds = new string[3] { "1", "2", "3" };
            filter.PatientIds = new ulong[3] { 1, 2, 3 };
            DbCommand command = builder.Build(filter, conn);
            Assert.IsTrue(command.CommandText.Equals(SQLRequestBuilder.Consts.RequestStart + " creation_timestamp >= @fromDate and creation_timestamp <= @toDate and ( id = ANY(@protoIds) AND patient_id = ANY(@patIds)) " + SQLRequestBuilder.Consts.Appendix + ";"));
            Assert.IsTrue(command.Parameters.Contains("protoIds"));
            Assert.IsTrue(command.Parameters.Contains("patIds"));
            Assert.IsTrue(command.Parameters.Contains("toDate"));
            Assert.IsTrue(command.Parameters.Contains("fromDate"));
        }

        [TestMethod]
        public void TestMethod7()
        {
            Filter filter = new Filter();
            filter.From = new DateTime(2010, 1, 1);
            filter.To = new DateTime(2010, 2, 1);
            filter.ProtocolsIds = new string[3] { "1", "2", "3" };
            filter.PatientIds = new ulong[3] { 1, 2, 3 };
            filter.HeadersIds = new string[3] { "1", "2", "3" };
            DbCommand command = builder.Build(filter, conn);
            Assert.IsTrue(command.CommandText.Equals(SQLRequestBuilder.Consts.RequestStart + " creation_timestamp >= @fromDate and creation_timestamp <= @toDate and ( id = ANY(@protoIds) AND patient_id = ANY(@patIds) AND header_id = ANY(@headersIds)) " + SQLRequestBuilder.Consts.Appendix + ";"));
            Assert.IsTrue(command.Parameters.Contains("protoIds"));
            Assert.IsTrue(command.Parameters.Contains("headersIds"));
            Assert.IsTrue(command.Parameters.Contains("patIds"));
            Assert.IsTrue(command.Parameters.Contains("toDate"));
            Assert.IsTrue(command.Parameters.Contains("fromDate"));
        }

        [TestMethod]
        public void TestMethod8()
        {
            Filter filter = new Filter();
            filter.From = new DateTime(2010, 1, 1);
            filter.To = new DateTime(2010, 2, 1);
            filter.ProtocolsIds = new string[3] { "1", "2", "3" };
            filter.ProtocolTypeIds = new ulong[3] { 1, 2, 3 };
            filter.PatientIds = new ulong[3] { 1, 2, 3 };
            filter.HeadersIds = new string[3] { "1", "2", "3" };
            DbCommand command = builder.Build(filter, conn);
            Assert.IsTrue(command.CommandText.Equals(SQLRequestBuilder.Consts.RequestStart + " creation_timestamp >= @fromDate and creation_timestamp <= @toDate and ( id = ANY(@protoIds) AND patient_id = ANY(@patIds) AND header_id = ANY(@headersIds) AND type = ANY(@typeIds)) " + SQLRequestBuilder.Consts.Appendix + ";"));
            Assert.IsTrue(command.Parameters.Contains("protoIds"));
            Assert.IsTrue(command.Parameters.Contains("headersIds"));
            Assert.IsTrue(command.Parameters.Contains("patIds"));
            Assert.IsTrue(command.Parameters.Contains("typeIds"));
            Assert.IsTrue(command.Parameters.Contains("toDate"));
            Assert.IsTrue(command.Parameters.Contains("fromDate"));
        }

        [TestMethod]
        public void TestMethod9()
        {
            Filter filter = new Filter();
            filter.From = new DateTime(2010, 1, 1);
            filter.To = new DateTime(2010, 2, 1);
            DbCommand command = builder.Build(filter, conn);
            Assert.IsTrue(command.CommandText.Equals(SQLRequestBuilder.Consts.RequestStart + " creation_timestamp >= @fromDate and creation_timestamp <= @toDate" + SQLRequestBuilder.Consts.Appendix + ";"));
            Assert.IsTrue(command.Parameters.Contains("toDate"));
            Assert.IsTrue(command.Parameters.Contains("fromDate"));
        }
    }
}
