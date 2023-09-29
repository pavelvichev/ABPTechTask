﻿using ABPTechTask.Controllers;
using Application.Experiments;
using Application.Results;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ABPTechTask.Tests.Controller;

public class ControllerValidPriceTests
{
    [Fact]
    public async Task PriceExperiment_DeviceHasNoExistingResult_ReturnsNewResult()
    {
        // Arrange
        var deviceToken = "myToken";

        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ReturnsAsync(new Experiment { Id = 1, Key = "price", Options = "{\"10\":0.75,\"20\":0.1,\"50\":0.05,\"5\":0.1}" });

        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindResult.Query>(), default))
            .ReturnsAsync((ExperimentResult)null); 
        
        
        var controller = new ExperimentsController(mediatorMock.Object);

        // Act
        
        var result = await controller.PriceExperimnt(deviceToken);
        
    
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseObject = Assert.IsType<Dictionary<string, string>>(okResult.Value); // Import System.Collections.Generic for Dictionary.
        Assert.Equal("price", responseObject["key"]);
        Assert.Contains(responseObject["value"], new[] {"10" , "20", "50", "5"});
        
    }
    
    [Fact]
    public async Task PriceExperiment_DeviceHasExistingResult_ReturnsExistingResult()
    {
        // Arrange
        var deviceToken = "myToken";

        // Mock the Mediator and its responses.
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ReturnsAsync(new Experiment { Id = 1, Key = "price", Options = "{\"10\":0.75,\"20\":0.1,\"50\":0.05,\"5\":0.1}"});

        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindResult.Query>(), default))
            .ReturnsAsync(new ExperimentResult { ExperimentId = 1, DeviceToken = deviceToken, Value = "10" }); // Simulate an existing result.

        var controller = new ExperimentsController(mediatorMock.Object);

        // Act
        var result = await controller.PriceExperimnt(deviceToken);

        // Assert
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseObject = Assert.IsType<Dictionary<string, string>>(okResult.Value); 
        Assert.Equal("price", responseObject["key"]);
        Assert.Equal("10", responseObject["value"]);
        
        
    }
    
    [Fact]
    public async Task PriceColorExperiment_DeviceHasExistingResult_ReturnsAlreadyParticipating()
    {
        // Arrange
        var deviceToken = "myToken";

        // Mock the Mediator and its responses.
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ReturnsAsync(new Experiment { Id = 2, Key = "price", Options = "{\"10\":0.75,\"20\":0.1,\"50\":0.05,\"5\":0.1}"});

        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindResult.Query>(), default))
            .ReturnsAsync(new ExperimentResult { ExperimentId = 1, DeviceToken = deviceToken, Value = "50" }); // Simulate an existing result.

        var controller = new ExperimentsController(mediatorMock.Object);

        // Act
        var result = await controller.PriceExperimnt(deviceToken);

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Пристрій вже бере участь в експерименті", okResult.Value);
    }
}