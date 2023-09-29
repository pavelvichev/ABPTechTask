using ABPTechTask.Controllers;
using Application.Experiments;
using Application.Results;
using Domain;
using EntityFrameworkCore.Testing.Moq;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistance;

namespace ABPTechTask.Tests.Controller;

public class ControllerValidButtonTests
{
    
    
    [Fact]
    public async Task ButtonColorExperiment_DeviceHasNoExistingResult_ReturnsNewResult()
    {
        // Arrange
        var deviceToken = "your-device-token";

        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ReturnsAsync(new Experiment { Id = 1, Key = "button_color", Options = "red,green,blue" });

        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindResult.Query>(), default))
            .ReturnsAsync((ExperimentResult)null); // Simulate no existing result.
        
        
        var controller = new ExperimentsController(mediatorMock.Object);

        // Act
        
        var result = await controller.ButtonColorExperimnt(deviceToken);
        
    
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseObject = Assert.IsType<Dictionary<string, string>>(okResult.Value); // Import System.Collections.Generic for Dictionary.
        Assert.Equal("button_color", responseObject["key"]);
        Assert.Contains(responseObject["value"], new[] {"blue" , "green", "red"});
        
    }
    
    [Fact]
    public async Task ButtonColorExperiment_DeviceHasExistingResult_ReturnsExistingResult()
    {
        // Arrange
        var deviceToken = "your-device-token";

        // Mock the Mediator and its responses.
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ReturnsAsync(new Experiment { Id = 1, Key = "button_color", Options = "red,green,blue"});

        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindResult.Query>(), default))
            .ReturnsAsync(new ExperimentResult { ExperimentId = 1, DeviceToken = deviceToken, Value = "green" }); // Simulate an existing result.

        var controller = new ExperimentsController(mediatorMock.Object);

        // Act
        var result = await controller.ButtonColorExperimnt(deviceToken);

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
        var deviceToken = "your-device-token";

        // Mock the Mediator and its responses.
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ReturnsAsync(new Experiment { Id = 2, Key = "button_color", Options = "red,green,blue"});

        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindResult.Query>(), default))
            .ReturnsAsync(new ExperimentResult { ExperimentId = 1, DeviceToken = deviceToken, Value = "green" }); // Simulate an existing result.

        var controller = new ExperimentsController(mediatorMock.Object);

        // Act
        var result = await controller.ButtonColorExperimnt(deviceToken);

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Should().BeEquivalentTo("Пристрій вже бере участь в експерименті");
    }
}