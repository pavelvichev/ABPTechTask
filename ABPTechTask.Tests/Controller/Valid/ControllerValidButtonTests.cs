using ABPTechTask.Controllers;
using Application.Experiments;
using Application.Results;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ABPTechTask.Tests.Controller.Valid;

public class ControllerValidButtonTests
{
    
    
    [Fact]
    public async Task ButtonColorExperiment_DeviceHasNoExistingResult_ReturnsNewResult()
    {
        // Arrange
        var deviceToken = "myToken";

        
        var mediatorMock = new Mock<IMediator>();
        var logger = new Mock<ILogger<ExperimentsController>>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ReturnsAsync(new Experiment { Id = 1, Key = "button_color", Options = "red,green,blue" });

        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindResult.Query>(), default))
            .ReturnsAsync((ExperimentResult)null);
        
        
        var controller = new ExperimentsController(mediatorMock.Object, logger.Object);

        // Act
        
        var result = await controller.ButtonColorExperiment(deviceToken);
        
    
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseObject = Assert.IsType<Dictionary<string, string>>(okResult.Value);
        Assert.Equal("button_color", responseObject["key"]);
        Assert.Contains(responseObject["value"], new[] {"blue" , "green", "red"});
        
    }
    
    [Fact]
    public async Task ButtonColorExperiment_DeviceHasExistingResult_ReturnsExistingResult()
    {
        // Arrange
        var deviceToken = "myToken";
        
        var mediatorMock = new Mock<IMediator>();
        var logger = new Mock<ILogger<ExperimentsController>>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ReturnsAsync(new Experiment { Id = 1, Key = "button_color", Options = "red,green,blue"});

        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindResult.Query>(), default))
            .ReturnsAsync(new ExperimentResult { ExperimentId = 1, DeviceToken = deviceToken, Value = "green" }); 

        var controller = new ExperimentsController(mediatorMock.Object, logger.Object);

        // Act
        var result = await controller.ButtonColorExperiment(deviceToken);

        // Assert
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseObject = Assert.IsType<Dictionary<string, string>>(okResult.Value); 
        Assert.Equal("button_color", responseObject["key"]);
        Assert.Equal("green", responseObject["value"]);
        
        
    }
    
    [Fact]
    public async Task ButtonColorExperiment_DeviceHasExistingResult_ReturnsAlreadyParticipating()
    {
        // Arrange
        var deviceToken = "myToken";

        
        var mediatorMock = new Mock<IMediator>();
        var logger = new Mock<ILogger<ExperimentsController>>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ReturnsAsync(new Experiment { Id = 2, Key = "button_color", Options = "red,green,blue"});

        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindResult.Query>(), default))
            .ReturnsAsync(new ExperimentResult { ExperimentId = 1, DeviceToken = deviceToken, Value = "green" });
        
        var controller = new ExperimentsController(mediatorMock.Object, logger.Object);

        // Act
        var result = await controller.ButtonColorExperiment(deviceToken);

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo("Пристрій вже бере участь в експерименті");
    }
}