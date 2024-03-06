using Xunit;
using Moq;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace WebApi.Hubs.Tests
{
    public class GameHubTests
    {
    [Fact]
        public async Task SendMessage_ValidUserConnection_SendsMessageToGroup()
        {
            // Arrange
            var connectionId = "testConnectionId";
            var userConnection = new UserConnection { User = "TestUser", Room = "TestRoom" };

            var userConnections = new Dictionary<string, UserConnection>();
            userConnections[connectionId] = userConnection;

            var mockClients = new Mock<IHubCallerClients>();
            var mockGroups = new Mock<IGroupManager>();
            var mockContext = new Mock<HubCallerContext>();
            mockContext.SetupGet(c => c.ConnectionId).Returns(connectionId);

            var gameHub = new GameHub(userConnections)
            {
                Clients = mockClients.Object,
                Groups = mockGroups.Object,
                Context = mockContext.Object
            };

            // Mock the Clients.Group.SendAsync method
            var mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(c => c.Group(It.IsAny<string>())).Returns(mockClientProxy.Object);

            // Act
            await gameHub.SendMessage("TestMessage");

            // ReceiveNessage is a vue function, i wonder if thats why?
            mockClientProxy.Verify(c => c.SendAsync("ReceiveMessage", It.IsAny<object[]>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }   
    }

}