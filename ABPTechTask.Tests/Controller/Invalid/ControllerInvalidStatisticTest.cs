using ABPTechTask.Controllers;
using Application.Experiments;
using Application.Statistic.Results;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ABPTechTask.Tests.Controller.Invalid;

public class ControllerInvalidStatisticTest
{
    [Fact]
    public async Task Statistic_ThrowException()
    {
        // Arrange
        
        var mediatorMock = new Mock<IMediator>();
        var logger = new Mock<ILogger<ExperimentsController>>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<ExperimentResultsCount.Query>(), default))
            .ThrowsAsync(new Exception("Simulated exception"));

        var controller = new ExperimentsController(mediatorMock.Object, logger.Object);
        
        //Act
        var result = await controller.GetStatistic();

        //Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}