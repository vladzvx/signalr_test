using Microsoft.VisualStudio.TestTools.UnitTesting;
using IASK.DataHub.Services;
using Moq;
using Microsoft.AspNetCore.SignalR;

namespace IASK.Test.DataHub
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();

           // mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
        }
    }
}
