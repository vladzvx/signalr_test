using IASK.InterviewerEngine.Models.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IASK.InterviewerEngineTests
{
    [TestClass]
    public class CheckerSupportingClassesTests
    {
        [TestMethod]
        public void Content_CheckStringValues_test()
        {
            string testString = "test string 123__ &&7789298930939839892-1-22````ЁЁЁёёёьвфьлыфльывфЬЬлывффдвлфдвлф";
            Assert.IsTrue(Content.CheckStringValues("", ""));
            Assert.IsFalse(Content.CheckStringValues(null, ""));
            Assert.IsFalse(Content.CheckStringValues(null, testString));
            Assert.IsFalse(Content.CheckStringValues("", null));
            Assert.IsFalse(Content.CheckStringValues(testString, null));
            Assert.IsTrue(Content.CheckStringValues(null, null));
            Assert.IsTrue(Content.CheckStringValues(testString, testString));

        }

        [TestMethod]
        public void Content_Equals_test1()
        {
            Content content1 = new Content();
            Content content2 = new Content();
            Assert.IsTrue(content1.Equals(content1));
            Assert.IsTrue(content2.Equals(content2));
            Assert.IsTrue(content1.Equals(content2));
            Assert.IsTrue(content2.Equals(content1));
        }

        [TestMethod]
        public void Content_Equals_test2()
        {
            Content content1 = new Content(Id: "1", Value: "112343545", ParentId: "qqqq");
            Content content2 = new Content(Id: "1", Value: "112343545", ParentId: "qqqq");
            Assert.IsTrue(content1.Equals(content1));
            Assert.IsTrue(content2.Equals(content2));
            Assert.IsTrue(content1.Equals(content2));
            Assert.IsTrue(content2.Equals(content1));
        }
        [TestMethod]
        public void Content_Equals_test3()
        {
            Content content1 = new Content(Id: "1", Value: "112343545", ParentId: null);
            Content content2 = new Content(Id: "1", Value: "112343545", ParentId: null);
            Assert.IsTrue(content1.Equals(content1));
            Assert.IsTrue(content2.Equals(content2));
            Assert.IsTrue(content1.Equals(content2));
            Assert.IsTrue(content2.Equals(content1));
        }

        [TestMethod]
        public void Content_Equals_test4()
        {
            Content content1 = new Content(Id: "1", Value: null, ParentId: "qqqq");
            Content content2 = new Content(Id: "1", Value: null, ParentId: "qqqq");
            Assert.IsTrue(content1.Equals(content1));
            Assert.IsTrue(content2.Equals(content2));
            Assert.IsTrue(content1.Equals(content2));
            Assert.IsTrue(content2.Equals(content1));
        }

        [TestMethod]
        public void Content_Equals_test5()
        {
            Content content1 = new Content(Id: null, Value: "112343545", ParentId: "qqqq");
            Content content2 = new Content(Id: null, Value: "112343545", ParentId: "qqqq");
            Assert.IsTrue(content1.Equals(content1));
            Assert.IsTrue(content2.Equals(content2));
            Assert.IsTrue(content1.Equals(content2));
            Assert.IsTrue(content2.Equals(content1));
        }


        [TestMethod]
        public void Content_Equals_test6()
        {
            Content content1 = new Content(Id: "1", Value: "112343545", ParentId: "qqqq");
            Content content2 = new Content(Id: null, Value: "112343545", ParentId: "qqqq");
            Assert.IsTrue(content1.Equals(content1));
            Assert.IsTrue(content2.Equals(content2));
            Assert.IsFalse(content1.Equals(content2));
            Assert.IsFalse(content2.Equals(content1));
        }

        [TestMethod]
        public void Content_Equals_test7()
        {
            Content content1 = new Content(Id: "1", Value: null, ParentId: "qqqq");
            Content content2 = new Content(Id: null, Value: "112343545", ParentId: "qqqq");
            Assert.IsTrue(content1.Equals(content1));
            Assert.IsTrue(content2.Equals(content2));
            Assert.IsFalse(content1.Equals(content2));
            Assert.IsFalse(content2.Equals(content1));
        }

    }
}
