using DevOpsTaskApp.Application.AzureDevOps.Commands.CreateAzureDevOpsTask;
using DevOpsTaskApp.Application.AzureDevOps.Application.Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using Moq.Protected;


namespace DevOpsTaskApp.Application.Tests.AzureDevOps.Commands;

[TestClass]
public class CreateAzureDevOpsTaskCommandHandlerTests
{
    [TestMethod]
    public async Task Handle_ShouldSendRequestAndReturnResponse()
    {
        // Arrange
        var expectedResponse = "{\"id\": 123, \"title\": \"Test Task\"}";
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handlerMock.Object);

        var optionsMock = new Mock<IOptions<AzureDevOpsSettings>>();
        optionsMock.Setup(o => o.Value).Returns(new AzureDevOpsSettings
        {
            BaseUrl = "https://dev.azure.com/",
            ApiVersion = "6.0"
        });

        var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<CreateAzureDevOpsTaskCommandHandler>>();
        var handler = new CreateAzureDevOpsTaskCommandHandler(httpClient, optionsMock.Object, loggerMock.Object);

        var command = new CreateAzureDevOpsTaskCommand
        {
            Organization = "YourOrg",
            Project = "YourProject",
            Title = "Test Task",
            IterationPath = "YourProject\\Iteration1",
            AssignedTo = "test@domain.com",
            UserStoryId = "123"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("\"id\": 123"));
    }
}
