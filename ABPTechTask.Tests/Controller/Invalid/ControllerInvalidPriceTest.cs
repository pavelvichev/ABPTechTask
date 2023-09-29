using ABPTechTask.Controllers;
using Application.Experiments;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ABPTechTask.Tests.Controller.Invalid;

public class ControllerInvalidPriceTest
{
    [Fact]
    public async Task PriceExperiment_ThrowException()
    {
        // Arrange
        var deviceToken = "myToken";

        
        var mediatorMock = new Mock<IMediator>();
        var logger = new Mock<ILogger<ExperimentsController>>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ThrowsAsync(new Exception("Simulated exception"));
        
        var controller = new ExperimentsController(mediatorMock.Object, logger.Object);

        //Act
        var result = await controller.PriceExperiment(deviceToken);
        
        //Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}